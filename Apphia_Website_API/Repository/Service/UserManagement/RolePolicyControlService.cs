using Microsoft.EntityFrameworkCore;
using Apphia_Website_API.Repository.Interface.UserManagement;
using Apphia_Website_API.Repository.Model.UserManagement;
using Apphia_Website_API.Repository.ViewModel.UserManagement;

namespace Apphia_Website_API.Repository.Service.UserManagement {
    public class RolePolicyControlService : IRolePolicyControlService {
        private readonly DatabaseContext _context;
        public RolePolicyControlService(DatabaseContext context) { _context = context; }

        public async Task<int> CreateRolePolicyControl(RolePolicyControlCreateViewModel rpc, int userId) {
            var entity = new RolePolicyControl { RoleId = rpc.RoleId, PolicyId = rpc.PolicyId, ControlId = rpc.ControlId, IsActive = true, CreatedByUserId = userId, CreatedDate = DateTime.Now };
            _context.RolePolicyControls.Add(entity);
            await _context.SaveChangesAsync();
            return entity.Id;
        }

        public async Task<List<RolePolicyControlReadViewModel>> GetRolePolicyControl(int roleId) {
            return await _context.RolePolicyControls.Where(rpc => rpc.RoleId == roleId && rpc.IsActive == true)
                .Select(rpc => new RolePolicyControlReadViewModel { Id = rpc.Id, RoleId = rpc.RoleId, PolicyId = rpc.PolicyId, ControlId = rpc.ControlId }).ToListAsync();
        }

        public async Task<bool> DeleteRPC(int rpcId, int userId) {
            var entity = await _context.RolePolicyControls.FindAsync(rpcId);
            if (entity == null) return false;
            entity.IsActive = false; entity.DeletedByUserId = userId; entity.DeletedDate = DateTime.Now;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<RolePolicyControlReadViewModel>> GetRolePolicyControlNotActive(int roleId) {
            return await _context.RolePolicyControls.Where(rpc => rpc.RoleId == roleId && rpc.IsActive == false)
                .Select(rpc => new RolePolicyControlReadViewModel { Id = rpc.Id, RoleId = rpc.RoleId, PolicyId = rpc.PolicyId, ControlId = rpc.ControlId }).ToListAsync();
        }

        public async Task<bool> RestorePRC(int rpcId, int userId) {
            var entity = await _context.RolePolicyControls.FindAsync(rpcId);
            if (entity == null) return false;
            entity.IsActive = true; entity.RestoredByUserId = userId; entity.RestoredDate = DateTime.Now;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<PolicyReadViewModel>> GetPolicyControlById(int policyId, int roleId) {
            return await (from rpc in _context.RolePolicyControls
                join p in _context.Policies on rpc.PolicyId equals p.Id
                join c in _context.Controls on rpc.ControlId equals c.Id
                where rpc.PolicyId == policyId && rpc.RoleId == roleId && rpc.IsActive == true
                select new PolicyReadViewModel {
                    Id = rpc.PolicyId,
                    Name = p.Name,
                    Controls = new ControlReadViewModel {
                        Id = c.Id,
                        Create = c.Create,
                        Read = c.Read,
                        Update = c.Update,
                        Delete = c.Delete,
                        Restore = c.Restore
                    }
                }).ToListAsync();
        }
    }
}
