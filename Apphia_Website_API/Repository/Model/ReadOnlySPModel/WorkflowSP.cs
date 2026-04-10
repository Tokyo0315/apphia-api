namespace Apphia_Website_API.Repository.Model.ReadOnlySPModel
{
    public class WorkflowSP : SPBaseModel
    {
        public string? WorkflowName { get; set; }
        public string? Description { get; set; }
        public string? Status { get; set; }
        public string? ApprovalType { get; set; }
    }
}
