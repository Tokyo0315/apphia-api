using Apphia_Website_API.Repository.ViewModel.UserManagement;

namespace Apphia_Website_API.Repository.Interface.UserManagement {
    public interface IControlService {
        Task<int> CreateControl(ControlCreateViewModel control, int userId);
        Task<bool> UpdateControl(ControlUpdateViewModel control, int controlId, int userId);
        Task<List<ControlReadViewModel>> GetControlsByIds(List<int> controlId);
        Task<bool> Delete(int controlId, int userId);
        Task<bool> Restore(int controlId, int userId);
    }
}
