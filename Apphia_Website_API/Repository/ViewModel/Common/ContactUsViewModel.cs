namespace Apphia_Website_API.Repository.ViewModel.Common
{
    public class ContactUsViewModel
    {
        public string? firstName { get; set; }
        public string? lastName { get; set; }
        public string? email { get; set; }
        public string? contactNo { get; set; }
        public string? inquiry { get; set; }
        public string? message { get; set; }
        public bool cookieAccepted { get; set; }
    }

    public class ContactUsEmailViewModel
    {
        public required string email { get; set; }
    }

    public class ContactReadViewModel
    {
        public int Id { get; set; }
        public string Inquiry { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string ContactNo { get; set; } = string.Empty;
        public bool? IsActive { get; set; }
        public int TotalCount { get; set; }
        public string? DeletedBy { get; set; }
        public string? DeletedDate { get; set; }
    }
}
