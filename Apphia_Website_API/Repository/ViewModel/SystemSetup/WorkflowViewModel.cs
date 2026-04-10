namespace Apphia_Website_API.Repository.ViewModel.SystemSetup
{
    public class WorkflowCreateViewModel
    {
        public string WorkflowName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string ApprovalType { get; set; } = string.Empty;
        public List<WorkflowApproverCreateViewModel> WorkflowApprovers { get; set; } = new();
    }

    public class WorkflowApproverCreateViewModel
    {
        public string? ApproverName { get; set; }
        public string? EmailAddress { get; set; }
        public int? ApprovalOrder { get; set; }
    }

    public class WorkflowUpdateViewModel : WorkflowCreateViewModel { }

    public class WorkflowReadViewModel
    {
        public int Id { get; set; }
        public string WorkflowName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Status { get; set; }
        public string ApprovalType { get; set; } = string.Empty;
        public List<WorkflowApproverCreateViewModel> WorkflowApprovers { get; set; } = new();
    }
}
