using Apphia_Website_API.Repository.Model.UserManagement;
using Apphia_Website_API.Repository.ViewModel.UserManagement;

namespace Apphia_Website_API.Repository.Interface.Validation.UserManagement
{
    public interface IUserAccountValidationService
    {
        bool LoginInputValidation(LoginRequestViewModel user);
        bool EmailAndLengthValidation(LoginRequestViewModel user);
        bool UserAccountValidation(UserAccount account);
    }
}
