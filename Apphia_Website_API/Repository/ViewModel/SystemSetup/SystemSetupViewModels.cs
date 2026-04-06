namespace Apphia_Website_API.Repository.ViewModel.SystemSetup {
    public class EmailRecipientViewModel {
        public string email { get; set; } = string.Empty;
        public string segment { get; set; } = string.Empty;
    }
    public class SectionFormattingCreateViewModel {
        public string? Tab { get; set; }
        public string? Name { get; set; }
        public int? TabOrder { get; set; }
    }
    public class SectionFormattingUpdateViewModel : SectionFormattingCreateViewModel { }
    public class SectionFormattingReadViewModel {
        public int Id { get; set; }
        public string Section { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int? SequenceOrder { get; set; }
        public string? ApprovalStatus { get; set; }
        public bool? IsActive { get; set; }
        public int TotalCount { get; set; }
        public string? DeletedBy { get; set; }
        public string? DeletedDate { get; set; }
    }
    public class SectionFormattingGrapesJSViewModel {
        public string? Html { get; set; }
        public string? Css { get; set; }
        public string? Js { get; set; }
        public string? Data { get; set; }
    }
    public class SectionFormattingTabs {
        public string Label { get; set; } = string.Empty;
        public string Route { get; set; } = string.Empty;
        public List<SectionFormattingTabs> Children { get; set; } = new();
    }
    public class WorkflowCreateViewModel {
        public string WorkflowName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string ApprovalType { get; set; } = string.Empty;
        public List<WorkflowApproverCreateViewModel> WorkflowApprovers { get; set; } = new();
    }
    public class WorkflowApproverCreateViewModel {
        public string? ApproverName { get; set; }
        public string? EmailAddress { get; set; }
        public int? ApprovalOrder { get; set; }
    }
    public class WorkflowUpdateViewModel : WorkflowCreateViewModel { }
    public class RequestUpdateSetupSecurityVM {
        public int? FailedAttempt { get; set; }
        public int? LockTimeOut { get; set; }
        public bool? IsDisableTimeOut { get; set; }
    }
    public class WorkflowReadViewModel {
        public int Id { get; set; }
        public string WorkflowName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Status { get; set; }
        public string ApprovalType { get; set; } = string.Empty;
        public List<WorkflowApproverCreateViewModel> WorkflowApprovers { get; set; } = new();
    }
}
