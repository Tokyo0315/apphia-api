using Apphia_Website_API.Repository.Interface;
using Apphia_Website_API.Repository.Interface.ApiCommand;
using Apphia_Website_API.Repository.Interface.Core;
using Apphia_Website_API.Repository.Interface.SystemSetup;
using Apphia_Website_API.Repository.Interface.Transaction;
using Apphia_Website_API.Repository.Interface.UserManagement;
using Apphia_Website_API.Repository.Model.Audit;
using Apphia_Website_API.Repository.Service;
using Apphia_Website_API.Repository.Service.ApiCommand;
using Apphia_Website_API.Repository.Service.AuditLog;
using Apphia_Website_API.Repository.Service.Core;
using Apphia_Website_API.Repository.Service.SystemSetup;
using Apphia_Website_API.Repository.Service.Transaction;
using Apphia_Website_API.Repository.Service.UserManagement;
using Apphia_Website_API.Utils;

namespace Apphia_Website_API.Extension;

public static class AuditExtension {
    public static IServiceCollection AddAuditServices(this IServiceCollection services) {
        // Audit services
        services.AddScoped(typeof(IAuditGenericService<>), typeof(AuditGenericService<>));
        services.AddScoped<IAuditServiceFactory, AuditServiceFactory>();

        // ApiCommand & Notification
        services.AddScoped<IApiCommandService, ApiCommandService>();
        services.AddScoped<IApiService, ApiService>();

        // Base
        services.AddScoped<IBaseService, BaseService>();

        // UserManagement
        services.AddScoped<IUserAccountService, UserAccountService>();
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<IControlService, ControlService>();
        services.AddScoped<IPolicyService, PolicyService>();
        services.AddScoped<IRolePolicyControlService, RolePolicyControlService>();

        // Core
        services.AddScoped<IEmployeeService, EmployeeService>();
        services.AddScoped<IDashboardService, DashboardService>();
        services.AddScoped<IApprovalService, ApprovalService>();

        // SystemSetup
        services.AddScoped<ISectionFormattingService, SectionFormattingService>();
        services.AddScoped<IWorkflowService, WorkflowService>();
        services.AddScoped<IWorkflowApproverService, WorkflowApproverService>();
        services.AddScoped<IEmailRecipientService, EmailRecipientService>();
        services.AddScoped<ISetupSecurityManagementService, SetupSecurityManagementService>();

        // Transaction
        services.AddScoped<IContactService, ContactService>();

        // NEW Apphia
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IProductCategoryService, ProductCategoryService>();
        services.AddScoped<IGalleryAlbumService, GalleryAlbumService>();
        services.AddScoped<IGalleryPhotoService, GalleryPhotoService>();

        return services;
    }
}
