namespace Apphia_Website_API.Repository.Model.UserManagement {
    public class SetupSecurityManagement : BaseModel {
        public int? FailedAttempt { get; set; }
        public int? LockTimeOut { get; set; }
        public bool? IsDisableTimeOut { get; set; }
    }
}
