using Apphia_Website_API.Repository.Interface;
using Apphia_Website_API.Repository.Model.Audit;

namespace Apphia_Website_API.Utils;

public enum AuditType {
    UserAccount,
    SectionFormatting,
    Role,
    SetupSecurity,
    EmailRecipient,
    Contact,
    // NEW Apphia audit types
    Product,
    ProductCategory,
    GalleryPhoto,
    GalleryAlbum,
}

public class AuditServiceFactory : IAuditServiceFactory {
    private readonly Dictionary<AuditType, IAuditGenericService> services;

    public AuditServiceFactory(
        IAuditGenericService<UserAccountAudit> userAccountAuditService,
        IAuditGenericService<SectionFormattingAudit> sectionFormattingAuditService,
        IAuditGenericService<RoleAudit> roleAuditService,
        IAuditGenericService<SetupSecurityAudit> setupSecurityAudit,
        IAuditGenericService<EmailRecipientAudit> emailRecipientAuditService,
        IAuditGenericService<ContactAudit> contactAuditService,
        // NEW Apphia audit services
        IAuditGenericService<ProductAudit> productAuditService,
        IAuditGenericService<ProductCategoryAudit> productCategoryAuditService,
        IAuditGenericService<GalleryPhotoAudit> galleryPhotoAuditService,
        IAuditGenericService<GalleryAlbumAudit> galleryAlbumAuditService
    ) {
        services = new Dictionary<AuditType, IAuditGenericService> {
            { AuditType.UserAccount, userAccountAuditService },
            { AuditType.SectionFormatting, sectionFormattingAuditService },
            { AuditType.Role, roleAuditService },
            { AuditType.SetupSecurity, setupSecurityAudit },
            { AuditType.EmailRecipient, emailRecipientAuditService },
            { AuditType.Contact, contactAuditService },
            // NEW
            { AuditType.Product, productAuditService },
            { AuditType.ProductCategory, productCategoryAuditService },
            { AuditType.GalleryPhoto, galleryPhotoAuditService },
            { AuditType.GalleryAlbum, galleryAlbumAuditService },
        };
    }

    public IAuditGenericService GetService(AuditType type) {
        return services[type];
    }
}
