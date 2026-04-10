using Apphia_Website_API.Repository.ViewModel.UserManagement;

namespace Apphia_Website_API.Repository.Interface.Validation.UserManagement
{
    public interface IRoleValidationService
    {
        bool RoleCreateValidation(RoleCreateViewModel role);
        bool RoleUpdateValidation(RoleUpdateViewModel role, int roleId);
    }
}
