using Apphia_Website_API.Repository.Interface;

namespace Apphia_Website_API.Repository.Service {
    public class BaseService : IBaseService {
        private readonly DatabaseContext _context;
        public BaseService(DatabaseContext context) {
            _context = context;
        }
    }
}
