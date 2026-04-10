namespace Apphia_Website_API.Repository.Interface
{
    public interface IAuditLogService
    {
        Task LogStatusToFileAsync(string message, string destination, string endpoint, string status);
    }
}
