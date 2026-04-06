namespace Apphia_Website_API.Repository.ViewModel.Response.NotificationApi {
    public class ResponseNotificationGetTokenVM {
        public int Status { get; set; }
        public bool Success { get; set; }
        public string? Message { get; set; }
        public NotificationGetTokenVM Data { get; set; } = new NotificationGetTokenVM();
    }

    public class NotificationGetTokenVM {
        public string? Token { get; set; }
        public DateTime? Expired { get; set; }
    }
}
