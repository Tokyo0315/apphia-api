using System.ComponentModel.DataAnnotations;

namespace Apphia_Website_API.Repository.Model.SystemSetup {
    public class SectionFormatting : BaseModel {
        public bool? HasContentChanges { get; set; }
        public bool? IsOnWorkflow { get; set; }
        public string? Tab { get; set; }
        public string? Name { get; set; }
        public int? TabOrder { get; set; }
        public string? Html { get; set; }
        public string? Css { get; set; }
        public string? Js { get; set; }
        public string? Data { get; set; }
    }

    public class SectionFormattingVersion : BaseModel {
        [Required]
        public int SectionFormattingId { get; set; }
        public string? Html { get; set; }
        public string? Css { get; set; }
        public string? Js { get; set; }
        public string? Data { get; set; }
        public string? ApprovalStatus { get; set; }
    }

    public class SectionFormattingVersionWorkflow : BaseModel {
        [Required]
        public int SectionFormattingVersionId { get; set; }
        public int? WorkflowId { get; set; }
    }

    public class SectionFormattingVersionWorkflowApprover : BaseModel {
        [Required]
        public int SectionFormattingVersionWorkflowId { get; set; }
        public string? ApproverName { get; set; }
        public string? EmailAddress { get; set; }
        public int? ApprovalOrder { get; set; }
        public string? ApprovalStatus { get; set; }
        public DateTime? ApprovalDate { get; set; }
    }
}
