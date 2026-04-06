using Microsoft.EntityFrameworkCore;
using Apphia_Website_API.Repository.Interface.SystemSetup;
using Apphia_Website_API.Repository.Model.UserManagement;

namespace Apphia_Website_API.Repository.Service.SystemSetup {
    public class SetupSecurityManagementService : ISetupSecurityManagementService {
        private readonly DatabaseContext _context;
        public SetupSecurityManagementService(DatabaseContext context) { _context = context; }

        public async Task<bool> UpdateAsync(SetupSecurityManagement model, int id, int userId) {
            var entity = await _context.SetupSecurityManagements.FindAsync(id);
            if (entity == null) return false;
            entity.FailedAttempt = model.FailedAttempt; entity.LockTimeOut = model.LockTimeOut; entity.IsDisableTimeOut = model.IsDisableTimeOut;
            entity.UpdatedByUserId = userId; entity.UpdatedDate = DateTime.Now;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<object> ReadSecurity() {
            return await _context.SetupSecurityManagements.Where(x => x.IsActive == true).ToListAsync();
        }

        public async Task<object> ReadOneSecurity(int id) {
            return (await _context.SetupSecurityManagements.FindAsync(id))!;
        }
    }
}
