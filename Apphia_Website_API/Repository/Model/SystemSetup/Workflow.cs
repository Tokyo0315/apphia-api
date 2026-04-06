using System.ComponentModel.DataAnnotations;

namespace Apphia_Website_API.Repository.Model.SystemSetup {
    public class Workflow : BaseModel {
        [Required]
        public string WorkflowName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int Status { get; set; }
        [Required]
        public string ApprovalType { get; set; } = string.Empty;
    }

    public class WorkflowApprover : BaseModel {
        [Required]
        public int WorkflowId { get; set; }
        public string? ApproverName { get; set; }
        public string? EmailAddress { get; set; }
        public int? ApprovalOrder { get; set; }
        public bool PermanentDelete { get; set; }
        public int? PermanentDeletedByUserId { get; set; }
        public DateTime? PermanentDeletedDate { get; set; }
    }
}
