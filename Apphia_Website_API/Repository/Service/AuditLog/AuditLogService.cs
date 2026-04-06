using Apphia_Website_API.Repository.Configuration.Helper;
using Apphia_Website_API.Repository.Interface;

namespace Apphia_Website_API.Repository.Service.AuditLog {
    public class AuditLogService : IAuditLogService {
        public async Task LogStatusToFileAsync(string message, string destination, string endpoint, string status) {
            await Task.Run(() => LogHelper.WriteLogToFile(message, destination, endpoint, status));
        }
    }
}
