using Apphia_Website_API.Repository.ViewModel.UserManagement;

namespace Apphia_Website_API.Repository.Interface.UserManagement {
    public interface IRolePolicyControlService {
        Task<int> CreateRolePolicyControl(RolePolicyControlCreateViewModel rolePolicyControl, int userId);
        Task<List<RolePolicyControlReadViewModel>> GetRolePolicyControl(int roleId);
        Task<bool> DeleteRPC(int rpcId, int userId);
        Task<List<RolePolicyControlReadViewModel>> GetRolePolicyControlNotActive(int roleId);
        Task<bool> RestorePRC(int rpcId, int userId);
        Task<List<PolicyReadViewModel>> GetPolicyControlById(int policyId, int roleId);
    }
}
