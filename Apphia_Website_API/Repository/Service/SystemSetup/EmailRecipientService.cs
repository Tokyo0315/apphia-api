using Microsoft.EntityFrameworkCore;
using Apphia_Website_API.Repository.Configuration.Exception_Extender;
using Apphia_Website_API.Repository.Interface.SystemSetup;
using Apphia_Website_API.Repository.Model.SystemSetup;
using Apphia_Website_API.Repository.ViewModel.SystemSetup;

namespace Apphia_Website_API.Repository.Service.SystemSetup {
    public class EmailRecipientService : IEmailRecipientService {
        private readonly DatabaseContext _context;
        public EmailRecipientService(DatabaseContext context) { _context = context; }

        public async Task<EmailRecipient> ReadRecipient(int emailId) {
            var entity = await _context.EmailRecipients.FindAsync(emailId);
            if (entity == null) throw new NotFoundResource();
            return entity;
        }

        public async Task<List<EmailRecipient>> ReadRecipients(int isActive, int pagenumber, int pagesize, string? filter, string? sort) {
            var query = _context.EmailRecipients.Where(e => e.IsActive == (isActive == 1));
            if (!string.IsNullOrEmpty(filter)) query = query.Where(e => e.email.Contains(filter) || e.segment.Contains(filter));
            return await query.Skip((pagenumber - 1) * pagesize).Take(pagesize).ToListAsync();
        }

        public async Task<List<EmailRecipient>> ReadRecipientBaseOnSegment(string segment) {
            return await _context.EmailRecipients.Where(e => e.segment == segment && e.IsActive == true).ToListAsync();
        }

        public async Task<EmailRecipient> InsertEmail(EmailRecipientViewModel vm, int userId) {
            var entity = new EmailRecipient { email = vm.email, segment = vm.segment, IsActive = true, CreatedByUserId = userId, CreatedDate = DateTime.Now };
            _context.EmailRecipients.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> UpdateEmail(EmailRecipientViewModel vm, int emailId, int userId) {
            var entity = await _context.EmailRecipients.FindAsync(emailId);
            if (entity == null) return false;
            entity.email = vm.email; entity.segment = vm.segment; entity.UpdatedByUserId = userId; entity.UpdatedDate = DateTime.Now;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteEmail(int emailId, int userId) {
            var entity = await _context.EmailRecipients.FindAsync(emailId);
            if (entity == null) return false;
            entity.IsActive = false; entity.DeletedByUserId = userId; entity.DeletedDate = DateTime.Now;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RestoreEmail(int emailId, int userId) {
            var entity = await _context.EmailRecipients.FindAsync(emailId);
            if (entity == null) return false;
            entity.IsActive = true; entity.RestoredByUserId = userId; entity.RestoredDate = DateTime.Now;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
