using Apphia_Website_API.Repository.Model.SystemSetup;
using Apphia_Website_API.Repository.ViewModel.SystemSetup;

namespace Apphia_Website_API.Repository.Interface.SystemSetup {
    public interface IWorkflowService {
        Task<Workflow> Create(WorkflowCreateViewModel viewModel, int userId);
        Task<List<Workflow>> Read(int isActive, int pagenumber, int pagesize, string? filter, string? sort);
        Task<WorkflowReadViewModel> ReadSF(int id);
        Task<string> Update(WorkflowUpdateViewModel viewModel, int id, int userId);
        Task<bool> Delete(int id, int userId);
        Task<bool> Restore(int id, int userId);
    }
    public interface IWorkflowApproverService {
        Task<bool> WorkflowApprover(int workflowId, string approvalType, List<WorkflowApproverCreateViewModel> viewModel, int userId);
        Task<List<WorkflowApproverCreateViewModel>> ReadSF(int workflowId);
    }
}
