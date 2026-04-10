using Apphia_Website_API.Repository.Interface.Validation.UserManagement;
using Apphia_Website_API.Repository.Model.UserManagement;
using Apphia_Website_API.Repository.ViewModel.UserManagement;

namespace Apphia_Website_API.Repository.Service.Validation.UserManagement
{
    public class UserAccountValidationService : IUserAccountValidationService
    {
        public bool LoginInputValidation(LoginRequestViewModel user)
        {
            if (string.IsNullOrWhiteSpace(user.Email) && string.IsNullOrWhiteSpace(user.Password))
                throw new Exception("Username and Password are required.");
            if (string.IsNullOrWhiteSpace(user.Email)) throw new Exception("Username is required.");
            if (!user.Email.Contains('@')) throw new Exception("Please provide a valid email.");
            if (string.IsNullOrWhiteSpace(user.Password)) throw new Exception("Password is required.");
            return true;
        }

        public bool EmailAndLengthValidation(LoginRequestViewModel user)
        {
            if (user.Password.Length < 8) throw new Exception("Password must be at least 8 characters long.");
            return true;
        }

        public bool UserAccountValidation(UserAccount account)
        {
            if (account.IsActive != true) throw new UnauthorizedAccessException("Email or Password is wrong");
            if (account.RequiredPasswordChange == true) throw new UnauthorizedAccessException("Password change is required. Please update your password.");
            return true;
        }
    }
}
