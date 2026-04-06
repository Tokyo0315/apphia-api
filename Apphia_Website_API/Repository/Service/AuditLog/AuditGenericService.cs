using Apphia_Website_API.Repository;
using Apphia_Website_API.Repository.Interface;
using Apphia_Website_API.Repository.Model.Audit;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Apphia_Website_API.Repository.Service.AuditLog {
    public class AuditGenericService<T> : IAuditGenericService<T> where T : BaseAuditModel {
        private readonly DatabaseContext _context;

        public AuditGenericService(DatabaseContext context) {
            _context = context;
        }

        public async Task CreateLog(BaseAuditModel entity, ClaimsPrincipal user) {
            await CreateLog((T)entity, user);
        }

        public async Task CreateLog(T entity, ClaimsPrincipal user) {
            var userId = user.FindFirst("UserId")?.Value ?? "";
            var roleId = user.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value ?? "";
            var roleName = user.FindFirst(System.Security.Claims.ClaimTypes.Name)?.Value ?? "";

            entity.ActionByUserId = userId;
            entity.ActionByRoleId = roleId;
            entity.ActionByRole = roleName;
            entity.CreatedDate = DateTime.Now;

            _context.Set<T>().Add(entity);
            await _context.SaveChangesAsync();
        }

        async Task<(List<BaseAuditModel> Logs, int TotalCount)> IAuditGenericService.GetLogs(DateTime? from, DateTime? to, string filter, int quantity, int page) {
            var result = await GetLogs(from, to, filter, quantity, page);
            return (result.Logs.Cast<BaseAuditModel>().ToList(), result.TotalCount);
        }

        public async Task<(List<T> Logs, int TotalCount)> GetLogs(DateTime? from, DateTime? to, string filter, int quantity, int page) {
            var query = _context.Set<T>().AsQueryable();
            if (from.HasValue) query = query.Where(l => l.CreatedDate >= from.Value);
            if (to.HasValue) query = query.Where(l => l.CreatedDate <= to.Value.AddDays(1));
            if (!string.IsNullOrEmpty(filter))
                query = query.Where(l => l.Action.Contains(filter) || l.Details.Contains(filter) || l.ActionByName.Contains(filter));
            var totalCount = await query.CountAsync();
            var logs = await query.OrderByDescending(l => l.CreatedDate).Skip((page - 1) * quantity).Take(quantity).ToListAsync();
            return (logs, totalCount);
        }

        async Task<List<BaseAuditModel>> IAuditGenericService.GetAllLogs(DateTime? from, DateTime? to) {
            var result = await GetAllLogs(from, to);
            return result.Cast<BaseAuditModel>().ToList();
        }

        public async Task<List<T>> GetAllLogs(DateTime? from, DateTime? to) {
            var query = _context.Set<T>().AsQueryable();
            if (from.HasValue) query = query.Where(l => l.CreatedDate >= from.Value);
            if (to.HasValue) query = query.Where(l => l.CreatedDate <= to.Value.AddDays(1));
            return await query.OrderByDescending(l => l.CreatedDate).ToListAsync();
        }
    }
}
