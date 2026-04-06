using Apphia_Website_API.Repository.Model.UserManagement;
using Apphia_Website_API.Repository.ViewModel.UserManagement;

namespace Apphia_Website_API.Repository.Interface.UserManagement {
    public interface IPolicyService {
        Task<int> GetPolicy(string name);
        Task<List<PolicyReadViewModel>> GetPoliciesByIds(List<int> policyId);
        Task<int> CreatePolicy(PolicyCreateViewModel policy, int userId);
        Task<List<Policy>> GetPolicyModelByPolicyId(int policyId);
    }
}
