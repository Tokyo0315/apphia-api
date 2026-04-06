using Microsoft.EntityFrameworkCore;
using Apphia_Website_API.Repository.Configuration;
using Apphia_Website_API.Repository.Configuration.Exception_Extender;
using Apphia_Website_API.Repository.Configuration.Helper;
using Apphia_Website_API.Repository.Interface;
using Apphia_Website_API.Repository.Interface.UserManagement;
using Apphia_Website_API.Repository.Model.Core;
using Apphia_Website_API.Repository.Model.UserManagement;
using Apphia_Website_API.Repository.ViewModel.UserManagement;
using System.Security.Cryptography;
using System.Text;

namespace Apphia_Website_API.Repository.Service.UserManagement {
    public class UserAccountService : IUserAccountService {
        private readonly DatabaseContext _context;
        private readonly IJwtHelper _jwtHelper;
        private readonly IRolePolicyControlService _rolePolicyControlService;
        private readonly IRoleService _roleService;
        private readonly IPolicyService _policyService;
        private readonly IControlService _controlService;
        private readonly IConfiguration _configuration;
        private readonly double DaysToExpire = 90;

        public UserAccountService(DatabaseContext context, IJwtHelper jwt, IRolePolicyControlService rolePolicyControlService, IRoleService roleService, IPolicyService policyService, IControlService controlService, IConfiguration configuration) {
            _context = context;
            _jwtHelper = jwt;
            _rolePolicyControlService = rolePolicyControlService;
            _roleService = roleService;
            _policyService = policyService;
            _controlService = controlService;
            _configuration = configuration;
        }

        public async Task<int> CreateUserAccount(UserAccountCreateViewModel userAccount, int userId) {
            var exist = await _context.Employees.AnyAsync(e => e.EmployeeNumber == userAccount.EmployeeNumber);
            if (exist) throw new InvalidOperationException("Employee Number already exist.");

            var exists = await _context.UserAccounts.AnyAsync(u => u.UserID == userAccount.Email);
            if (exists) throw new InvalidOperationException("Email already exists.");

            var employee = new Employee {
                EmployeeNumber = userAccount.EmployeeNumber,
                LastName = userAccount.LastName,
                GivenName = userAccount.GivenName,
                MiddleName = userAccount.MiddleName,
                Email = userAccount.Email,
                IsActive = true,
                CreatedByUserId = userId,
                CreatedDate = DateTime.Now
            };
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            var password = new RandomHelper().GenerateRandomString(8);
            var salt = GenerateSalt();
            var hashedPassword = HashPassword(password, salt);

            var account = new UserAccount {
                UserID = userAccount.Email,
                PasswordSalt = hashedPassword,
                Salt = salt,
                ExpiryDate = DateTime.Now.AddDays(DaysToExpire),
                EmployeeId = employee.Id,
                RoleId = userAccount.RoleId,
                IsActive = true,
                IsLocked = false,
                CreatedByUserId = userId,
                CreatedDate = DateTime.Now,
            };
            _context.UserAccounts.Add(account);
            await _context.SaveChangesAsync();

            var tempPassword = new TempPassword {
                tempPassword = password,
                UserID = account.Id,
                IsActive = true,
                CreatedByUserId = userId,
                CreatedDate = DateTime.Now
            };
            _context.TempPasswords.Add(tempPassword);
            await _context.SaveChangesAsync();

            return account.Id;
        }

        public async Task<List<UserAccountReadViewModel>> GetUserAccount(int isActive, int pagenumber, int pagesize, string? filter, string? sort) {
            var query = _context.UserAccounts
                .Include(u => u.Employee).Include(u => u.Role)
                .Where(u => u.IsActive == (isActive == 1));

            if (!string.IsNullOrEmpty(filter))
                query = query.Where(u => u.UserID.Contains(filter) || u.Employee.LastName.Contains(filter) || u.Employee.GivenName.Contains(filter));

            var totalCount = await query.CountAsync();
            var users = await query.Skip((pagenumber - 1) * pagesize).Take(pagesize).ToListAsync();

            return users.Select(u => {
                var givenName = u.Employee?.GivenName ?? "";
                var lastName = u.Employee?.LastName ?? "";
                var name = string.IsNullOrEmpty(lastName) ? givenName : $"{givenName} {lastName}".Trim();

                return new UserAccountReadViewModel {
                    Id = u.Id, UserID = u.UserID, Email = u.UserID,
                    EmployeeNumber = u.Employee?.EmployeeNumber ?? "",
                    Name = name,
                    GivenName = givenName, LastName = lastName,
                    MiddleName = u.Employee?.MiddleName,
                    RoleId = u.RoleId, Role = u.Role?.Name ?? "",
                    ExpiryDate = u.ExpiryDate.ToString("MMMM d, yyyy"),
                    IsLocked = u.IsLocked == true ? "Yes" : "No",
                    IsActive = u.IsActive, TotalCount = totalCount,
                    DeletedBy = u.DeletedByUserId != null ? _context.UserAccounts.Include(x => x.Employee).Where(x => x.Id == u.DeletedByUserId).Select(x => x.Employee!.GivenName + " " + x.Employee.LastName).FirstOrDefault() : null,
                    DeletedDate = u.DeletedDate?.ToString("MMMM d, yyyy"),
                };
            }).ToList();
        }

        public async Task<UserAccount?> GetOneUser(int userId) {
            return await _context.UserAccounts.Include(u => u.Employee).Include(u => u.Role).FirstOrDefaultAsync(e => e.Id == userId);
        }

        public async Task UpdateUserAccount(int id, UserAccountUpdateViewModel updatedAccount, int userId) {
            var account = await _context.UserAccounts.Include(ua => ua.Employee).FirstOrDefaultAsync(ua => ua.Id == id);
            if (account == null) throw new KeyNotFoundException("User account not found.");
            if (account.Employee == null) throw new InvalidOperationException("Associated employee record not found.");

            var duplicateEmpNum = await _context.Employees.AnyAsync(e => e.EmployeeNumber == updatedAccount.EmployeeNumber && e.Id != id);
            if (duplicateEmpNum) throw new InvalidOperationException("Employee Number already exists.");

            var duplicate = await _context.UserAccounts.AnyAsync(u => u.UserID == updatedAccount.Email && u.Id != id);
            if (duplicate) throw new InvalidOperationException("Email already exists.");

            account.Employee.EmployeeNumber = updatedAccount.EmployeeNumber;
            account.Employee.LastName = updatedAccount.LastName;
            account.Employee.GivenName = updatedAccount.GivenName;
            account.Employee.MiddleName = updatedAccount.MiddleName;
            account.Employee.Email = updatedAccount.Email;
            account.UserID = updatedAccount.Email;
            account.RoleId = updatedAccount.RoleId;

            if (!string.IsNullOrEmpty(updatedAccount.Password)) {
                var salt = GenerateSalt();
                account.Salt = salt;
                account.PasswordSalt = HashPassword(updatedAccount.Password, salt);
            }
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUserAccount(int id, int userId) {
            var account = await _context.UserAccounts.FindAsync(id);
            if (account == null) throw new Exception("User account not found.");
            if (account.IsActive == false) throw new InvalidOperationException("User has already been deleted.");
            account.IsActive = false;
            account.DeletedByUserId = userId;
            account.DeletedDate = DateTime.Now;
            await _context.SaveChangesAsync();
        }

        public async Task RestoreUserAccount(int id, int userId) {
            var account = await _context.UserAccounts.FindAsync(id);
            if (account == null) throw new Exception("User account not found.");
            if (account.IsActive == true) throw new InvalidOperationException("User has already been restored.");
            account.IsActive = true;
            account.DeletedByUserId = null;
            account.DeletedDate = null;
            await _context.SaveChangesAsync();
        }

        private string GenerateSalt() {
            byte[] saltBytes = new byte[16];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(saltBytes);
            return Convert.ToBase64String(saltBytes);
        }

        public string HashPassword(string password, string salt) {
            using var sha256 = SHA256.Create();
            var combined = Encoding.UTF8.GetBytes(password + salt);
            var hash = sha256.ComputeHash(combined);
            return Convert.ToBase64String(hash);
        }

        public async Task<Employee> GetEmployeeByEmail(string email) {
            var userInfo = await _context.Employees.FirstOrDefaultAsync(c => c.Email == email);
            if (userInfo == null) throw new NotFoundResource("Email not found");
            return userInfo;
        }

        public async Task<AuthenticateResponse> AuthenticateUser(LoginRequestViewModel user) {
            var userInfo = await _context.UserAccounts.FirstOrDefaultAsync(s => s.UserID == user.Email && s.IsActive == true);
            var setupSecurityList = await _context.SetupSecurityManagements.Where(x => x.IsActive!.Value).ToListAsync();

            if (userInfo == null) throw new UnauthorizedAccessException("Invalid username or password.");

            var setupSecurity = setupSecurityList.FirstOrDefault();

            if (userInfo.LockedDate != null && userInfo.LockedDate < DateTime.Now) {
                userInfo.IsLocked = false; userInfo.FailedAttempt = 0; userInfo.LockedDate = null;
                await _context.SaveChangesAsync();
            }
            if (userInfo.LockedDate != null && userInfo.LockedDate > DateTime.Now)
                throw new UnauthorizedAccessException($"Account is locked. Please wait Until {userInfo.LockedDate}.");
            if (userInfo.RequiredPasswordChange == true)
                throw new UnauthorizedAccessException("User account requires password change.");
            if (userInfo.ExpiryDate < DateTime.UtcNow) {
                userInfo.RequiredPasswordChange = true;
                await _context.SaveChangesAsync();
                throw new UnauthorizedAccessException("User account requires password change.");
            }

            var salt = userInfo.Salt;
            var hashedPassword = HashPassword(user.Password, salt);
            if (userInfo.PasswordSalt != hashedPassword) {
                ++userInfo.FailedAttempt;
                if (setupSecurity != null && userInfo.FailedAttempt >= setupSecurity.FailedAttempt)  {
                    userInfo.IsLocked = true;
                    userInfo.LockedDate = DateTime.Now.AddMinutes(setupSecurity.LockTimeOut!.Value);
                }
                await _context.SaveChangesAsync();
                throw new UnauthorizedAccessException("Invalid username or password.");
            }
            userInfo.FailedAttempt = 0;
            await _context.SaveChangesAsync();

            var employee = await GetEmployeeByEmail(user.Email);
            var rpcList = await _rolePolicyControlService.GetRolePolicyControl(userInfo.RoleId);
            var policyIds = rpcList.Select(rpc => rpc.PolicyId).Distinct().ToList();
            var controlIds = rpcList.Select(rpc => rpc.ControlId).Distinct().ToList();
            var policies = await _policyService.GetPoliciesByIds(policyIds);
            var controls = await _controlService.GetControlsByIds(controlIds);
            var role = await _roleService.GetRoleById(userInfo.RoleId);

            foreach (var rpc in rpcList) {
                var policy = policies.FirstOrDefault(p => p.Id == rpc.PolicyId);
                var control = controls.FirstOrDefault(c => c.Id == rpc.ControlId);
                if (policy != null && control != null) {
                    role.PolicyControls.Add(new PolicyReadViewModel {
                        Id = rpc.Id, Name = policy.Name,
                        Controls = new ControlReadViewModel { Id = control.Id, Create = control.Create, Read = control.Read, Update = control.Update, Delete = control.Delete, Restore = control.Restore }
                    });
                }
            }

            var token = _jwtHelper.GenerateJwtToken(userInfo.Id.ToString(), role);
            return new AuthenticateResponse {
                userId = userInfo.Id, Token = token,
                UserFirstname = employee.GivenName, UserLastname = employee.LastName,
                RoleName = role.Name, EmployeeId = employee.Id, roleId = role.Id
            };
        }

        public async Task<UserAccount> GetById(int id) {
            return (await _context.UserAccounts.FirstOrDefaultAsync(c => c.Id == id && c.IsActive == true))!;
        }

        public async Task<bool> LinkValidity(string userId, string token) {
            var result = await _context.PasswordResetRequests.AnyAsync(x => x.UserAccountId == Convert.ToInt32(userId) && x.Token == token && x.IsUsed != true && x.ExpirationDate > DateTime.Now);
            if (!result) throw new Exception("Invalid or expired link.");
            return true;
        }

        public async Task<Dictionary<string, string>> ResetPasswordLink(string email) {
            var userId = await _context.UserAccounts.Where(x => x.UserID == email).Select(x => x.Id).FirstOrDefaultAsync();
            if (userId == 0) throw new Exception("If the email is associated with an account, a password reset link will be sent.");

            var exist = await _context.PasswordResetRequests.Where(x => x.UserAccountId == userId && x.IsUsed != true && x.IsActive == true).FirstOrDefaultAsync();
            if (exist != null) { exist.IsUsed = true; exist.IsActive = false; exist.ExpirationDate = DateTime.Now; await _context.SaveChangesAsync(); }

            var settings = _configuration.GetSection("HttpContextSettings").Get<HttpContextSettings>();
            string baseUrl = settings?.DevelopmentMode == true ? "https://localhost:4201/change-password/" : "/change-password/";

            string genSalt = GenerateSalt();
            string genToken = HashPassword(userId.ToString(), genSalt);
            byte[] tokenBytes = Encoding.UTF8.GetBytes(genToken);
            string safeToken = Convert.ToBase64String(tokenBytes).Replace("+", "-").Replace("/", "_").Replace("=", "");

            baseUrl += userId + "/" + safeToken;

            var newRequest = new PasswordResetRequest { UserAccountId = userId, Token = safeToken, IsUsed = false, IsActive = true, ExpirationDate = DateTime.Now.AddMinutes(15) };
            _context.PasswordResetRequests.Add(newRequest);
            await _context.SaveChangesAsync();

            return new Dictionary<string, string> { { "baseUrl", baseUrl }, { "expire", newRequest.ExpirationDate.ToString() } };
        }

        public async Task<Dictionary<string, string>> ChangePassword(int userId, string password, string token) {
            var user = await _context.UserAccounts.FirstOrDefaultAsync(x => x.Id == userId && x.IsActive == true);
            if (user == null) throw new NotFoundResource();

            var genSalt = GenerateSalt();
            user.Salt = genSalt;
            user.PasswordSalt = HashPassword(password, genSalt);
            user.RequiredPasswordChange = false;
            user.ExpiryDate = DateTime.Now.AddDays(DaysToExpire);
            await _context.SaveChangesAsync();

            var request = await _context.PasswordResetRequests.FirstOrDefaultAsync(x => x.UserAccountId == userId && x.Token == token);
            if (request != null) { request.IsUsed = true; request.ExpirationDate = DateTime.Now; await _context.SaveChangesAsync(); }

            return new Dictionary<string, string> { { "userID", user.UserID } };
        }
    }
}
