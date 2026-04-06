namespace Apphia_Website_API.Repository.ViewModel.Request.NotificationApi {
    public class RequestNotificationSendEmailVM {
        public string? Recipient { get; set; }
        public string? TemplateCode { get; set; }
        public string? ClientId { get; set; }
        public string? TemplateParameter { get; set; }
    }
}
