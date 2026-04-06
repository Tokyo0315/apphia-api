using Apphia_Website_API.Repository.ViewModel.Request.NotificationApi;

namespace Apphia_Website_API.Repository.Interface {
    public interface IApiService {
        Task<bool> NotificationSendEmail(RequestNotificationSendEmailVM request);
        Task<object> Send(string recipient, string templateCode, object body);
    }
}
