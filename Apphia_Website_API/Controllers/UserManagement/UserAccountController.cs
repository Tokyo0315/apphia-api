using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Apphia_Website_API.Repository.Interface;
using Apphia_Website_API.Repository.Interface.UserManagement;
using Apphia_Website_API.Repository.Interface.Core;
using Apphia_Website_API.Repository.ViewModel.UserManagement;
using Apphia_Website_API.Repository.Model.Audit;
using Apphia_Website_API.Repository.Configuration;
using Apphia_Website_API.Repository.Configuration.Attribute_Extender;
using Apphia_Website_API.Repository.Configuration.Enum;

namespace Apphia_Website_API.Controllers.UserManagement {
    [Route("api/[controller]")]
    [ApiController]
    public class UserAccountController : BaseController {
        private readonly IUserAccountService _userAccountService;
        private readonly IRoleService _roleService;
        private readonly IJwtHelper _jwtHelper;
        private readonly IAuditGenericService<UserAccountAudit> _auditService;

        public UserAccountController(
            IUserAccountService userAccountService,
            IRoleService roleService,
            IJwtHelper jwtHelper,
            IAuditGenericService<UserAccountAudit> auditService,
            IConfiguration configuration,
            IAuditLogService auditLogService,
            IRequestStatusHelper requestStatusHelper
        ) : base(configuration, auditLogService, requestStatusHelper) {
            _userAccountService = userAccountService;
            _roleService = roleService;
            _jwtHelper = jwtHelper;
            _auditService = auditService;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestViewModel loginRequest) {
            try {
                var result = await _userAccountService.AuthenticateUser(loginRequest);
                if (result == null)
                    return StatusCode(401, _requestStatusHelper.response(401, false, "Invalid email or password", null, null));

                var httpContextSettings = _configuration.GetSection("HttpContextSettings").Get<HttpContextSettings>();

                var cookieOptions = new CookieOptions {
                    HttpOnly = httpContextSettings!.HttpOnly,
                    Secure = httpContextSettings.Secure,
                    SameSite = httpContextSettings.SameSite.ToLower() switch {
                        "none" => SameSiteMode.None,
                        "lax" => SameSiteMode.Lax,
                        "strict" => SameSiteMode.Strict,
                        _ => SameSiteMode.Lax
                    },
                    Expires = DateTime.UtcNow.AddMinutes(httpContextSettings.ExpiresInMinutes)
                };

                HttpContext.Response.Cookies.Append("AccessToken", result.Token, cookieOptions);

                string sharedKeyPlain = result.userId + "|" + result.roleId;

                string aesKey = "apphiawebsiteapiadminside@2026th";
                string aesIv = "sharedkeyvectors";
                string encryptedSharedKey = AesEncrypt(sharedKeyPlain, aesKey, aesIv);

                var response = new {
                    result.userId,
                    result.UserFirstname,
                    result.UserLastname,
                    result.RoleName,
                    result.EmployeeId,
                    result.roleId,
                    Shared = encryptedSharedKey
                };

                await _auditService.CreateLog(new UserAccountAudit {
                    Action = "Login",
                    Details = "User " + loginRequest.Email + " logged in successfully"
                }, HttpContext.User);

                return StatusCode(200, _requestStatusHelper.response(200, true, "Login Successful", response, null));
            } catch (Exception ex) {
                return await HandleException(ex, "UserAccountController.Login", 500, "Internal Server Error");
            }
        }

        [Authorize]
        [HttpGet("Logout")]
        public IActionResult Logout() {
            try {
                var httpContextSettings = _configuration.GetSection("HttpContextSettings").Get<HttpContextSettings>();

                var cookieOptions = new CookieOptions {
                    HttpOnly = httpContextSettings!.HttpOnly,
                    Secure = httpContextSettings.Secure,
                    SameSite = httpContextSettings.SameSite.ToLower() switch {
                        "none" => SameSiteMode.None,
                        "lax" => SameSiteMode.Lax,
                        "strict" => SameSiteMode.Strict,
                        _ => SameSiteMode.Lax
                    },
                    Expires = DateTime.UtcNow.AddDays(-1)
                };

                HttpContext.Response.Cookies.Append("AccessToken", "", cookieOptions);
                return StatusCode(200, _requestStatusHelper.response(200, true, "Logout Successful", null, null));
            } catch (Exception ex) {
                return StatusCode(500, _requestStatusHelper.response(500, false, "Internal Server Error", ex.Message, null));
            }
        }

        [Authorize]
        [Control(AccessType.Create, Policies.UserAccount)]
        [HttpPost("Create/{userId}")]
        public async Task<IActionResult> Create([FromBody] UserAccountCreateViewModel model, int userId) {
            try {
                var result = await _userAccountService.CreateUserAccount(model, userId);

                await _auditService.CreateLog(new UserAccountAudit {
                    Action = "Create",
                    Details = "Created user account for " + model.Email
                }, HttpContext.User);

                return StatusCode(200, _requestStatusHelper.response(200, true, "User Account created successfully", result, null));
            } catch (Exception ex) {
                return await HandleException(ex, "UserAccountController.Create", 500, "Internal Server Error");
            }
        }

        [Authorize]
        [Control(AccessType.Read, Policies.UserAccount)]
        [HttpGet("Read")]
        public async Task<IActionResult> Read([FromQuery] int isActive, [FromQuery] int pageNumber, [FromQuery] int pageSize, [FromQuery] string? filter, [FromQuery] string? sort) {
            try {
                var result = await _userAccountService.GetUserAccount(isActive, pageNumber, pageSize, filter, sort);
                int? totalCount = result.Count > 0 ? result[0].TotalCount : 0;
                return StatusCode(200, _requestStatusHelper.response(200, true, "Success", result, totalCount));
            } catch (Exception ex) {
                return await HandleException(ex, "UserAccountController.Read", 500, "Internal Server Error");
            }
        }

        [Authorize]
        [Control(AccessType.Read, Policies.UserAccount)]
        [HttpGet("ReadIndividual/{userId}")]
        public async Task<IActionResult> ReadIndividual(int userId) {
            try {
                var result = await _userAccountService.GetOneUser(userId);
                if (result == null)
                    return StatusCode(404, _requestStatusHelper.response(404, false, "User not found", null, null));

                var dto = new {
                    result.Id,
                    result.Employee?.EmployeeNumber,
                    result.Employee?.GivenName,
                    result.Employee?.MiddleName,
                    result.Employee?.LastName,
                    Email = result.UserID,
                    result.RoleId,
                    RoleName = result.Role?.Name,
                    expiryDate = result.ExpiryDate.Date.ToString("yyyy-MM-dd"),
                };

                return StatusCode(200, _requestStatusHelper.response(200, true, "Success", dto, null));
            } catch (Exception ex) {
                return await HandleException(ex, "UserAccountController.ReadIndividual", 500, "Internal Server Error");
            }
        }

        [Authorize]
        [Control(AccessType.Update, Policies.UserAccount)]
        [HttpPost("Update/{id}/{userId}")]
        public async Task<IActionResult> Update([FromBody] UserAccountUpdateViewModel model, int id, int userId) {
            try {
                await _userAccountService.UpdateUserAccount(id, model, userId);

                await _auditService.CreateLog(new UserAccountAudit {
                    Action = "Update",
                    Details = "Updated user account ID: " + id
                }, HttpContext.User);

                return StatusCode(200, _requestStatusHelper.response(200, true, "User Account updated successfully", null, null));
            } catch (Exception ex) {
                return await HandleException(ex, "UserAccountController.Update", 500, "Internal Server Error");
            }
        }

        [Authorize]
        [Control(AccessType.Delete, Policies.UserAccount)]
        [HttpPost("Delete/{id}/{userId}")]
        public async Task<IActionResult> Delete(int id, int userId) {
            try {
                await _userAccountService.DeleteUserAccount(id, userId);

                await _auditService.CreateLog(new UserAccountAudit {
                    Action = "Delete",
                    Details = "Deleted user account ID: " + id
                }, HttpContext.User);

                return StatusCode(200, _requestStatusHelper.response(200, true, "User Account deleted successfully", null, null));
            } catch (Exception ex) {
                return await HandleException(ex, "UserAccountController.Delete", 500, "Internal Server Error");
            }
        }

        [Authorize]
        [Control(AccessType.Restore, Policies.UserAccount)]
        [HttpPost("Restore/{id}/{userId}")]
        public async Task<IActionResult> Restore(int id, int userId) {
            try {
                await _userAccountService.RestoreUserAccount(id, userId);

                await _auditService.CreateLog(new UserAccountAudit {
                    Action = "Restore",
                    Details = "Restored user account ID: " + id
                }, HttpContext.User);

                return StatusCode(200, _requestStatusHelper.response(200, true, "User Account restored successfully", null, null));
            } catch (Exception ex) {
                return await HandleException(ex, "UserAccountController.Restore", 500, "Internal Server Error");
            }
        }

        [HttpPost("forgot-password/{email}")]
        public async Task<IActionResult> ForgotPassword(string email) {
            try {
                var result = await _userAccountService.ResetPasswordLink(email);
                return StatusCode(200, _requestStatusHelper.response(200, true, "Password reset link sent", result, null));
            } catch (Exception ex) {
                return await HandleException(ex, "UserAccountController.ForgotPassword", 500, "Internal Server Error");
            }
        }

        [HttpGet("reset")]
        public async Task<IActionResult> ResetPasswordValidity([FromQuery] string userId, [FromQuery] string token) {
            try {
                var isValid = await _userAccountService.LinkValidity(userId, token);
                if (!isValid)
                    return StatusCode(400, _requestStatusHelper.response(400, false, "Invalid or expired reset link", null, null));
                return StatusCode(200, _requestStatusHelper.response(200, true, "Link is valid", null, null));
            } catch (Exception ex) {
                return await HandleException(ex, "UserAccountController.ResetPasswordValidity", 500, "Internal Server Error");
            }
        }

        [HttpPost("Success/{userId}")]
        public async Task<IActionResult> ChangePassword(int userId, [FromBody] UserAccountChangePassword model) {
            try {
                var result = await _userAccountService.ChangePassword(userId, model.Password, model.Token);
                return StatusCode(200, _requestStatusHelper.response(200, true, "Password changed successfully", result, null));
            } catch (Exception ex) {
                return await HandleException(ex, "UserAccountController.ChangePassword", 500, "Internal Server Error");
            }
        }

        [Authorize]
        [HttpGet("GetAllRole")]
        public async Task<IActionResult> GetAllRole() {
            try {
                var result = await _roleService.GetAll();
                return StatusCode(200, _requestStatusHelper.response(200, true, "Success", result, null));
            } catch (Exception ex) {
                return await HandleException(ex, "UserAccountController.GetAllRole", 500, "Internal Server Error");
            }
        }

        private static string AesEncrypt(string plainText, string key, string iv) {
            using var aes = Aes.Create();
            aes.Key = Encoding.UTF8.GetBytes(key);
            aes.IV = Encoding.UTF8.GetBytes(iv);
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            using var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
            byte[] encryptedBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);
            return Convert.ToBase64String(encryptedBytes);
        }
    }
}
