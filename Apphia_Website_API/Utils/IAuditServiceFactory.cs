using Apphia_Website_API.Repository.Interface;

namespace Apphia_Website_API.Utils
{
    public interface IAuditServiceFactory
    {
        IAuditGenericService GetService(AuditType type);
    }
}
