using Microsoft.EntityFrameworkCore;
using Apphia_Website_API.Repository.Configuration.Exception_Extender;
using Apphia_Website_API.Repository.Interface.Transaction;
using Apphia_Website_API.Repository.Model.Transaction;
using Apphia_Website_API.Repository.ViewModel.Transaction;
using Apphia_Website_API.Utils;

namespace Apphia_Website_API.Repository.Service.Transaction {
    public class ContactService : IContactService {
        private readonly DatabaseContext _context;
        private readonly IAesService _aesService;
        public ContactService(DatabaseContext context, IAesService aesService) { _context = context; _aesService = aesService; }

        public async Task<bool> Create(ContactUsViewModel contact) {
            var insert = new Contact {
                IsActive = true, cookieAccepted = contact.cookieAccepted,
                firstName = contact.firstName, lastName = contact.lastName, email = contact.email,
                contactNo = _aesService.Encrypt(contact.contactNo ?? ""), inquiry = contact.inquiry,
                message = _aesService.Encrypt(contact.message ?? ""), CreatedDate = DateTime.UtcNow
            };
            _context.Contacts.Add(insert);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<ContactReadViewModel>> ReadContacts(int pagenumber, int pagesize, int isActive, string? filter, string? sort) {
            var query = _context.Contacts.Where(c => c.IsActive == (isActive == 1));
            if (!string.IsNullOrEmpty(filter)) query = query.Where(c => c.firstName!.Contains(filter) || c.lastName!.Contains(filter) || c.email!.Contains(filter));

            var totalCount = await query.CountAsync();
            var items = await query.OrderByDescending(c => c.CreatedDate).Skip((pagenumber - 1) * pagesize).Take(pagesize).ToListAsync();

            return items.Select(c => new ContactReadViewModel {
                Id = c.Id,
                Inquiry = c.inquiry ?? "",
                Name = $"{c.firstName} {c.lastName}".Trim(),
                Email = c.email ?? "",
                ContactNo = c.contactNo ?? "",
                IsActive = c.IsActive,
                TotalCount = totalCount,
                DeletedBy = c.DeletedByUserId != null ? _context.UserAccounts.Include(x => x.Employee).Where(x => x.Id == c.DeletedByUserId).Select(x => x.Employee!.GivenName + " " + x.Employee.LastName).FirstOrDefault() : null,
                DeletedDate = c.DeletedDate?.ToString("MMMM d, yyyy"),
            }).ToList();
        }

        public async Task<Contact> ReadContact(int contactId) {
            var contact = await _context.Contacts.FindAsync(contactId);
            if (contact == null) throw new NotFoundResource();
            return contact;
        }

        public async Task<bool> DeleteContact(int contactId, int userId) {
            var contact = await _context.Contacts.FindAsync(contactId);
            if (contact == null) return false;
            contact.IsActive = false;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RestoreContact(int contactId, int userId) {
            var contact = await _context.Contacts.FindAsync(contactId);
            if (contact == null) return false;
            contact.IsActive = true;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
