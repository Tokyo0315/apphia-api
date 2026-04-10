using Apphia_Website_API.Repository.ViewModel.Common;

namespace Apphia_Website_API.Repository.Service.Validation.Common
{
    public class ContactUsValidation
    {
        public bool ContactUsInput(ContactUsViewModel contact)
        {
            if (string.IsNullOrWhiteSpace(contact.firstName)) throw new Exception("FirstName is required");
            if (string.IsNullOrWhiteSpace(contact.lastName)) throw new Exception("LastName is required");
            if (string.IsNullOrWhiteSpace(contact.email)) throw new Exception("Email is required");
            if (string.IsNullOrWhiteSpace(contact.inquiry)) throw new Exception("Inquiry is required");
            if (string.IsNullOrWhiteSpace(contact.message)) throw new Exception("Message is required");
            return true;
        }
    }
}
