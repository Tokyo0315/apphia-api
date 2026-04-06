using Microsoft.EntityFrameworkCore;
using Apphia_Website_API.Repository.Interface.UserManagement;
using Apphia_Website_API.Repository.Model.UserManagement;
using Apphia_Website_API.Repository.ViewModel.UserManagement;

namespace Apphia_Website_API.Repository.Service.UserManagement {
    public class ControlService : IControlService {
        private readonly DatabaseContext _context;
        public ControlService(DatabaseContext context) { _context = context; }

        public async Task<int> CreateControl(ControlCreateViewModel control, int userId) {
            var entity = new Control { Create = control.Create, Read = control.Read, Update = control.Update, Delete = control.Delete, Restore = control.Restore, IsActive = true, CreatedByUserId = userId, CreatedDate = DateTime.Now };
            _context.Controls.Add(entity);
            await _context.SaveChangesAsync();
            return entity.Id;
        }

        public async Task<bool> UpdateControl(ControlUpdateViewModel control, int controlId, int userId) {
            var entity = await _context.Controls.FindAsync(controlId);
            if (entity == null) return false;
            entity.Create = control.Create; entity.Read = control.Read; entity.Update = control.Update; entity.Delete = control.Delete; entity.Restore = control.Restore;
            entity.UpdatedByUserId = userId; entity.UpdatedDate = DateTime.Now;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<ControlReadViewModel>> GetControlsByIds(List<int> controlIds) {
            return await _context.Controls.Where(c => controlIds.Contains(c.Id))
                .Select(c => new ControlReadViewModel { Id = c.Id, Create = c.Create, Read = c.Read, Update = c.Update, Delete = c.Delete, Restore = c.Restore }).ToListAsync();
        }

        public async Task<bool> Delete(int controlId, int userId) {
            var entity = await _context.Controls.FindAsync(controlId);
            if (entity == null) return false;
            entity.IsActive = false; entity.DeletedByUserId = userId; entity.DeletedDate = DateTime.Now;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Restore(int controlId, int userId) {
            var entity = await _context.Controls.FindAsync(controlId);
            if (entity == null) return false;
            entity.IsActive = true; entity.RestoredByUserId = userId; entity.RestoredDate = DateTime.Now;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
