using Apphia_Website_API.Repository.Model.Core;
using Apphia_Website_API.Repository.Model.UserManagement;
using Apphia_Website_API.Repository.ViewModel.UserManagement;

namespace Apphia_Website_API.Repository.Interface.UserManagement {
    public interface IUserAccountService {
        Task<UserAccount> GetById(int id);
        Task<int> CreateUserAccount(UserAccountCreateViewModel userAccount, int userId);
        Task UpdateUserAccount(int id, UserAccountUpdateViewModel updatedAccount, int userId);
        Task DeleteUserAccount(int id, int userId);
        Task RestoreUserAccount(int id, int userId);
        string HashPassword(string password, string salt);
        Task<AuthenticateResponse> AuthenticateUser(LoginRequestViewModel user);
        Task<Employee> GetEmployeeByEmail(string email);
        Task<List<UserAccountReadViewModel>> GetUserAccount(int isActive, int pagenumber, int pagesize, string? filter, string? sort);
        Task<UserAccount?> GetOneUser(int userId);
        Task<Dictionary<string, string>> ResetPasswordLink(string email);
        Task<bool> LinkValidity(string userId, string token);
        Task<Dictionary<string, string>> ChangePassword(int userId, string password, string token);
    }
}
