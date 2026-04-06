using Apphia_Website_API.Repository.Model.Audit;
using System.Security.Claims;

namespace Apphia_Website_API.Repository.Interface {
    public interface IBaseService { }

    public interface IRequestStatusHelper {
        object response(int status, bool success, string? message, object? content, int? totalCount);
    }

    public interface IAuditLogService {
        Task LogStatusToFileAsync(string message, string destination, string endpoint, string status);
    }

    public interface IAuditGenericService {
        Task CreateLog(BaseAuditModel entity, ClaimsPrincipal user);
        Task<(List<BaseAuditModel> Logs, int TotalCount)> GetLogs(DateTime? from = null, DateTime? to = null, string filter = "", int quantity = 10, int page = 1);
        Task<List<BaseAuditModel>> GetAllLogs(DateTime? from = null, DateTime? to = null);
    }

    public interface IAuditGenericService<T> : IAuditGenericService where T : BaseAuditModel {
        Task CreateLog(T entity, ClaimsPrincipal user);
        new Task<(List<T> Logs, int TotalCount)> GetLogs(DateTime? from = null, DateTime? to = null, string filter = "", int quantity = 10, int page = 1);
        new Task<List<T>> GetAllLogs(DateTime? from = null, DateTime? to = null);
    }

    public interface IJwtHelper {
        string GenerateJwtToken(string userId, ViewModel.UserManagement.RoleReadViewModel role);
        string TokenDecoder(HttpContext context);
    }

    public interface IAuditServiceFactory {
        IAuditGenericService GetService(Utils.AuditType type);
    }
}
