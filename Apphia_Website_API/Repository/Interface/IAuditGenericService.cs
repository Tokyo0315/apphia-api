using Apphia_Website_API.Repository.Model.Audit;
using System.Security.Claims;

namespace Apphia_Website_API.Repository.Interface
{
    public interface IAuditGenericService
    {
        Task CreateLog(BaseAuditModel entity, ClaimsPrincipal user);
        Task<(List<BaseAuditModel> Logs, int TotalCount)> GetLogs(DateTime? from = null, DateTime? to = null, string filter = "", int quantity = 10, int page = 1);
        Task<List<BaseAuditModel>> GetAllLogs(DateTime? from = null, DateTime? to = null);
    }

    public interface IAuditGenericService<T> : IAuditGenericService where T : BaseAuditModel
    {
        Task CreateLog(T entity, ClaimsPrincipal user);
        new Task<(List<T> Logs, int TotalCount)> GetLogs(DateTime? from = null, DateTime? to = null, string filter = "", int quantity = 10, int page = 1);
        new Task<List<T>> GetAllLogs(DateTime? from = null, DateTime? to = null);
    }
}
