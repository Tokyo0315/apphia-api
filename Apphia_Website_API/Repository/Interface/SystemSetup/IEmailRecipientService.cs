using Apphia_Website_API.Repository.Model.SystemSetup;
using Apphia_Website_API.Repository.ViewModel.SystemSetup;

namespace Apphia_Website_API.Repository.Interface.SystemSetup {
    public interface IEmailRecipientService {
        Task<EmailRecipient> ReadRecipient(int emailId);
        Task<List<EmailRecipient>> ReadRecipients(int isActive, int pagenumber, int pagesize, string? filter, string? sort);
        Task<List<EmailRecipient>> ReadRecipientBaseOnSegment(string segment);
        Task<EmailRecipient> InsertEmail(EmailRecipientViewModel recipient, int userId);
        Task<bool> UpdateEmail(EmailRecipientViewModel recipient, int emailId, int userId);
        Task<bool> DeleteEmail(int emailId, int userId);
        Task<bool> RestoreEmail(int emailId, int userId);
    }
}
