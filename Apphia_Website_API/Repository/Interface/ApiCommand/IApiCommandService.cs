using ApiCommandModel = Apphia_Website_API.Repository.Model.ApiCommand.ApiCommand;

namespace Apphia_Website_API.Repository.Interface.ApiCommand {
    public interface IApiCommandService {
        Task<List<ApiCommandModel>> GetAllActiveAsync();
        Task<ApiCommandModel> GetActiveByIdAsync(int id);
        Task<bool> UpdateValueAsync(string value, int id);
    }
}
