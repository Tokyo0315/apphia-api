namespace Apphia_Website_API.Repository.Configuration.Enum {
    public enum AccessType {
        Create = 1,
        Update = 2,
        Delete = 3,
        Read = 4,
        Restore = 5,
    }

    public enum Policies {
        Role = 1,
        UserAccount = 2,
        SectionFormatting = 3,
        Workflow = 4,
        EmailRecipient = 5,
        CareerVacancies = 6,
        Applicants = 7,
        Contact = 8,
        AuditTrail = 9,
        // NEW Apphia policies
        Product = 10,
        ProductCategory = 11,
        GalleryPhoto = 12,
        GalleryAlbum = 13,
    }
}
