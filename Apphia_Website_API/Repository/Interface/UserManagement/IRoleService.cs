using Apphia_Website_API.Repository.Model.UserManagement;
using Apphia_Website_API.Repository.ViewModel.UserManagement;

namespace Apphia_Website_API.Repository.Interface.UserManagement {
    public interface IRoleService {
        Task<int> CreateRole(RoleCreateViewModel role, int userId);
        Task<List<RoleReadViewModel>> ReadRole(int isActive, int pagenumber, int pagesize, string? filter, string? sort);
        Task<Role> UpdateRole(RoleUpdateViewModel role, int roleId, int userId);
        Task DeleteRole(int roleId, int userId);
        Task RestoreRole(int roleId, int userId);
        Task<RoleReadViewModel> GetRoleById(int roleId);
        Task<Role?> GetByName(string name);
        Task<object> GetAll();
    }
}
