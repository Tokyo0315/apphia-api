using Microsoft.EntityFrameworkCore;
using Apphia_Website_API.Repository.Configuration.Exception_Extender;
using Apphia_Website_API.Repository.Interface.UserManagement;
using Apphia_Website_API.Repository.Model.UserManagement;
using Apphia_Website_API.Repository.ViewModel.UserManagement;

namespace Apphia_Website_API.Repository.Service.UserManagement {
    public class RoleService : IRoleService {
        private readonly DatabaseContext _context;

        public RoleService(DatabaseContext context) {
            _context = context;
        }

        public async Task<int> CreateRole(RoleCreateViewModel role, int userId) {
            var exists = await _context.Roles.AnyAsync(r => r.Name == role.Name || r.Code == role.Code);
            if (exists) throw new Exception("Role with the same name or code already exists.");

            var response = new Role { Code = role.Code, Name = role.Name, Description = role.Description, IsActive = true, CreatedByUserId = userId, CreatedDate = DateTime.Now };
            _context.Roles.Add(response);
            await _context.SaveChangesAsync();
            return response.Id;
        }

        public async Task<RoleReadViewModel> GetRoleById(int roleId) {
            var role = await _context.Roles.FirstOrDefaultAsync(x => x.IsActive == true && x.Id == roleId);
            if (role == null) throw new Exception($"Role with ID {roleId} not found.");

            var policyControls = await _context.RolePolicyControls
                .Where(rpc => rpc.RoleId == roleId && rpc.IsActive == true)
                .Include(rpc => rpc.Policy)
                .Include(rpc => rpc.Control)
                .Select(rpc => new PolicyReadViewModel {
                    Id = rpc.Id,
                    Name = rpc.Policy != null ? rpc.Policy.Name : null,
                    Controls = rpc.Control != null ? new ControlReadViewModel {
                        Id = rpc.Control.Id,
                        Create = rpc.Control.Create,
                        Read = rpc.Control.Read,
                        Update = rpc.Control.Update,
                        Delete = rpc.Control.Delete,
                        Restore = rpc.Control.Restore
                    } : new ControlReadViewModel()
                }).ToListAsync();

            return new RoleReadViewModel {
                Id = role.Id, Name = role.Name, Code = role.Code, Description = role.Description,
                PolicyControls = policyControls
            };
        }

        public async Task<Role?> GetByName(string name) => await _context.Roles.FirstOrDefaultAsync(r => r.Name == name);

        public async Task<List<RoleReadViewModel>> ReadRole(int isActive, int pagenumber, int pagesize, string? filter, string? sort) {
            var query = _context.Roles.Where(r => r.IsActive == (isActive == 1));
            if (!string.IsNullOrEmpty(filter)) query = query.Where(r => r.Name.Contains(filter) || r.Code.Contains(filter));
            return await query.Skip((pagenumber - 1) * pagesize).Take(pagesize)
                .Select(r => new RoleReadViewModel { Id = r.Id, Name = r.Name, Code = r.Code, Description = r.Description, IsActive = r.IsActive }).ToListAsync();
        }

        public async Task<object> GetAll() => await _context.Roles.Where(x => x.IsActive == true).Select(x => new { Key = x.Id, Label = x.Name }).ToListAsync();

        public async Task<Role> UpdateRole(RoleUpdateViewModel role, int roleId, int userId) {
            var roles = await _context.Roles.FindAsync(roleId);
            if (roles == null) throw new NotFoundResource($"Role with ID {roleId} is not found");
            var exists = await _context.Roles.AnyAsync(r => (r.Name == role.Name || r.Code == role.Code) && roleId != r.Id);
            if (exists) throw new Exception("Role with the same name or code already exists.");
            roles.Code = role.Code; roles.Name = role.Name; roles.Description = role.Description;
            roles.UpdatedDate = DateTime.Now; roles.UpdatedByUserId = userId;
            await _context.SaveChangesAsync();
            return roles;
        }

        public async Task DeleteRole(int roleId, int userId) {
            var role = await _context.Roles.FindAsync(roleId);
            if (role == null) throw new NotFoundResource($"Role with ID {roleId} not found.");
            if (role.IsActive == false) throw new DbUpdateException($"{role.Code} is already deleted.");
            bool inUse = await _context.UserAccounts.AnyAsync(u => u.RoleId == roleId);
            if (inUse) throw new DbUpdateException($"Role currently in use.");
            role.IsActive = false; role.DeletedByUserId = userId; role.DeletedDate = DateTime.Now;
            await _context.SaveChangesAsync();
        }

        public async Task RestoreRole(int roleId, int userId) {
            var role = await _context.Roles.FindAsync(roleId);
            if (role == null) throw new NotFoundResource($"Role with ID {roleId} not found.");
            if (role.IsActive == true) throw new DbUpdateException($"{role.Code} is already restored.");
            role.IsActive = true; role.RestoredByUserId = userId; role.RestoredDate = DateTime.Now;
            await _context.SaveChangesAsync();
        }
    }
}
