using Apphia_Website_API.Repository.Model.UserManagement;

namespace Apphia_Website_API.Repository.Interface.SystemSetup {
    public interface ISetupSecurityManagementService {
        Task<bool> UpdateAsync(SetupSecurityManagement model, int id, int userId);
        Task<object> ReadSecurity();
        Task<object> ReadOneSecurity(int id);
    }
}
