using Microsoft.EntityFrameworkCore;
using Apphia_Website_API.Repository.Interface.UserManagement;
using Apphia_Website_API.Repository.Model.UserManagement;
using Apphia_Website_API.Repository.ViewModel.UserManagement;

namespace Apphia_Website_API.Repository.Service.UserManagement {
    public class PolicyService : IPolicyService {
        private readonly DatabaseContext _context;
        public PolicyService(DatabaseContext context) { _context = context; }

        public async Task<int> GetPolicy(string name) {
            var policy = await _context.Policies.FirstOrDefaultAsync(p => p.Name == name);
            if (policy != null) return policy.Id;
            var newPolicy = new Policy { Name = name, IsActive = true, CreatedDate = DateTime.Now };
            _context.Policies.Add(newPolicy);
            await _context.SaveChangesAsync();
            return newPolicy.Id;
        }

        public async Task<List<PolicyReadViewModel>> GetPoliciesByIds(List<int> policyIds) {
            return await _context.Policies.Where(p => policyIds.Contains(p.Id))
                .Select(p => new PolicyReadViewModel { Id = p.Id, Name = p.Name }).ToListAsync();
        }

        public async Task<int> CreatePolicy(PolicyCreateViewModel policy, int userId) {
            var entity = new Policy { Name = policy.Name, IsActive = true, CreatedByUserId = userId, CreatedDate = DateTime.Now };
            _context.Policies.Add(entity);
            await _context.SaveChangesAsync();
            return entity.Id;
        }

        public async Task<List<Policy>> GetPolicyModelByPolicyId(int policyId) {
            return await _context.Policies.Where(p => p.Id == policyId).ToListAsync();
        }
    }
}
