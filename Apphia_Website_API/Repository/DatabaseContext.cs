using Microsoft.EntityFrameworkCore;
using Apphia_Website_API.Repository.Model;
using Apphia_Website_API.Repository.Model.Audit;
using Apphia_Website_API.Repository.Model.Core;
using Apphia_Website_API.Repository.Model.SystemSetup;
using Apphia_Website_API.Repository.Model.Transaction;
using Apphia_Website_API.Repository.Model.UserManagement;

namespace Apphia_Website_API.Repository {
    public class DatabaseContext : DbContext {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) {
        }

        #region ========== Core Module ==========

        public DbSet<Model.ApiCommand.ApiCommand> ApiCommands { get; set; }
        public DbSet<PasswordResetRequest> PasswordResetRequests { get; set; }

        public DbSet<DashboardPage> DashboardPages { get; set; }
        public DbSet<DashboardCountry> DashboardCountries { get; set; }
        public DbSet<DashboardDevice> DashboardDevices { get; set; }
        public DbSet<DashboardEngagement> DashboardEngagements { get; set; }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Approval> Approvals { get; set; }

        #endregion

        #region ========== User Management ==========

        public DbSet<Role> Roles { get; set; }
        public DbSet<Policy> Policies { get; set; }
        public DbSet<Control> Controls { get; set; }
        public DbSet<RolePolicyControl> RolePolicyControls { get; set; }

        public DbSet<UserAccount> UserAccounts { get; set; }
        public DbSet<TempPassword> TempPasswords { get; set; }

        public DbSet<SetupSecurityManagement> SetupSecurityManagements { get; set; }

        #endregion

        #region ========== System Setup ==========

        public DbSet<SectionFormatting> SectionFormattings { get; set; }
        public DbSet<SectionFormattingVersion> SectionFormattingVersions { get; set; }
        public DbSet<SectionFormattingVersionWorkflow> SectionFormattingVersionWorkflows { get; set; }
        public DbSet<SectionFormattingVersionWorkflowApprover> SectionFormattingVersionWorkflowApprovers { get; set; }

        public DbSet<Workflow> Workflows { get; set; }
        public DbSet<WorkflowApprover> WorkflowApprovers { get; set; }

        #endregion

        #region ========== Transactions (SSCGI Recycled) ==========

        public DbSet<EmailRecipient> EmailRecipients { get; set; }
        public DbSet<Contact> Contacts { get; set; }

        #endregion

        #region ========== Transactions (NEW Apphia) ==========

        public DbSet<Product> Products { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<GalleryPhoto> GalleryPhotos { get; set; }
        public DbSet<GalleryAlbum> GalleryAlbums { get; set; }

        #endregion

        #region ========== Reports / Audit ==========

        public DbSet<UserAccountAudit> UserAccountAudits { get; set; }
        public DbSet<SectionFormattingAudit> SectionFormattingAudits { get; set; }
        public DbSet<RoleAudit> RoleAudits { get; set; }
        public DbSet<SetupSecurityAudit> SetupSecurityAudits { get; set; }
        public DbSet<WorkflowAudit> WorkflowAudits { get; set; }
        public DbSet<EmailRecipientAudit> EmailRecipientAudits { get; set; }
        public DbSet<ContactAudit> ContactAudits { get; set; }
        // NEW Apphia Audits
        public DbSet<ProductAudit> ProductAudits { get; set; }
        public DbSet<ProductCategoryAudit> ProductCategoryAudits { get; set; }
        public DbSet<GalleryPhotoAudit> GalleryPhotoAudits { get; set; }
        public DbSet<GalleryAlbumAudit> GalleryAlbumAudits { get; set; }


        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            // NEW: Product → ProductCategory relationship
            modelBuilder.Entity<Product>()
                .HasOne(p => p.ProductCategory)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.ProductCategoryId);

            // NEW: GalleryPhoto → GalleryAlbum relationship
            modelBuilder.Entity<GalleryPhoto>()
                .HasOne(p => p.GalleryAlbum)
                .WithMany(a => a.Photos)
                .HasForeignKey(p => p.GalleryAlbumId);
        }
    }
}
