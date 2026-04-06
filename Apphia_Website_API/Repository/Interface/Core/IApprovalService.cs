using Apphia_Website_API.Repository.ViewModel.Core;

namespace Apphia_Website_API.Repository.Interface.Core {
    public interface IApprovalService {
        Task<ApprovalViewModel> GetToken(string token);
        Task<string> ApprovalProcess(string token, ApprovalProcessViewModel approval);
    }
}
