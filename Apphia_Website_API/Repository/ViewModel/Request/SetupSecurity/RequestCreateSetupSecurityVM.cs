namespace Apphia_Website_API.Repository.ViewModel.Request.SetupSecurity
{
    public class RequestCreateSetupSecurityVM
    {
        public int? FailedAttempt { get; set; }
        public int? LockTimeOut { get; set; }
        public bool? IsDisableTimeOut { get; set; }
    }
}
