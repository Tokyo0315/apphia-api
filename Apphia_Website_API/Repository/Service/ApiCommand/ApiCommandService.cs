using Microsoft.EntityFrameworkCore;
using Apphia_Website_API.Repository.Interface.ApiCommand;
using ApiCommandModel = Apphia_Website_API.Repository.Model.ApiCommand.ApiCommand;

namespace Apphia_Website_API.Repository.Service.ApiCommand {
    public class ApiCommandService : IApiCommandService {
        private readonly DatabaseContext _context;

        public ApiCommandService(DatabaseContext context) {
            _context = context;
        }

        public async Task<List<ApiCommandModel>> GetAllActiveAsync() {
            return await _context.ApiCommands
                .Where(x => x.IsActive == true)
                .ToListAsync();
        }

        public async Task<ApiCommandModel> GetActiveByIdAsync(int id) {
            var result = await _context.ApiCommands
                .Where(c => c.Id == id && c.IsActive == true)
                .FirstOrDefaultAsync();
            return result ?? new ApiCommandModel();
        }

        public async Task<bool> UpdateValueAsync(string value, int id) {
            var data = await GetActiveByIdAsync(id);
            if (data == null || data.Id == 0) return false;
            data.Value = value;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
