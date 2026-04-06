using Apphia_Website_API.Repository.Model.Transaction;
using Apphia_Website_API.Repository.ViewModel.Transaction;

namespace Apphia_Website_API.Repository.Interface.Transaction {
    public interface IContactService {
        Task<bool> Create(ContactUsViewModel contact);
        Task<List<ContactReadViewModel>> ReadContacts(int pagenumber, int pagesize, int isActive, string? filter, string? sort);
        Task<Contact> ReadContact(int contactId);
        Task<bool> DeleteContact(int contactId, int userId);
        Task<bool> RestoreContact(int contactId, int userId);
    }
}
