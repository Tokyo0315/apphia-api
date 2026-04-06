using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Apphia_Website_API.Repository.Interface;
using Apphia_Website_API.Repository.Interface.ApiCommand;
using Apphia_Website_API.Repository.ViewModel.Request.NotificationApi;
using Apphia_Website_API.Repository.ViewModel.Response.NotificationApi;

namespace Apphia_Website_API.Repository.Service {
    public class ApiService : IApiService {
        private readonly DatabaseContext _context;
        private readonly IApiCommandService _apiCommandService;

        public ApiService(DatabaseContext context, IApiCommandService apiCommandService) {
            _context = context;
            _apiCommandService = apiCommandService;
        }

        private async Task<ResponseNotificationGetTokenVM> NotificationGetToken(string baseUrl, string clientId, string clientKey, int expiredId, int tokenId) {
            var url = $"{baseUrl}/Auth/GetToken";

            using var client = new HttpClient();
            var httpRequest = new HttpRequestMessage(HttpMethod.Post, url);

            var requestParam = new { clientId, clientKey };
            string jsonString = JsonSerializer.Serialize(requestParam);
            httpRequest.Content = new StringContent(jsonString, Encoding.UTF8, "application/json");

            var response = await client.SendAsync(httpRequest);
            response.EnsureSuccessStatusCode();

            var responseData = await response.Content.ReadAsStringAsync();
            var deserializedResponseData = JsonSerializer.Deserialize<ResponseNotificationGetTokenVM>(responseData, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                ?? new ResponseNotificationGetTokenVM();

            await _apiCommandService.UpdateValueAsync(deserializedResponseData.Data.Token!, tokenId);
            await _apiCommandService.UpdateValueAsync(deserializedResponseData.Data.Expired.ToString()!, expiredId);

            return deserializedResponseData;
        }

        public async Task<bool> NotificationSendEmail(RequestNotificationSendEmailVM request) {
            var list = await _apiCommandService.GetAllActiveAsync();
            if (!list.Any()) return false;

            var expiredDateList = list.Where(x => x.Name == "Expired").ToList();
            if (!expiredDateList.Any()) return false;

            var expiredDate = expiredDateList.FirstOrDefault();
            var clientIds = list.Where(x => x.Name == "ClientId").ToList();
            var clientKeys = list.Where(x => x.Name == "ClientKey").ToList();
            var baseUrls = list.Where(x => x.Name == "BaseUrl").ToList();
            var tokens = list.Where(x => x.Name == "Token").ToList();

            if (!clientIds.Any() || !clientKeys.Any() || !baseUrls.Any() || !tokens.Any())
                return false;

            var clientId = clientIds.FirstOrDefault();
            var clientKey = clientKeys.FirstOrDefault();
            var baseUrl = baseUrls.FirstOrDefault();
            var token = tokens.FirstOrDefault();

            var tokenResult = new ResponseNotificationGetTokenVM();
            var convertExpiredDate = DateTime.Parse(expiredDate!.Value!);

            if (DateTime.Now >= convertExpiredDate) {
                tokenResult = await NotificationGetToken(baseUrl!.Value!, clientId!.Value!, clientKey!.Value!, expiredDate.Id, token!.Id);
            } else {
                tokenResult.Data.Token = token!.Value;
            }

            var url = $"{baseUrl!.Value}/Email/SaveOutboxNotification";

            using var client = new HttpClient();
            var httpRequest = new HttpRequestMessage(HttpMethod.Post, url);
            httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokenResult.Data.Token);

            var requestParam = new {
                request.Recipient,
                request.TemplateCode,
                request.ClientId,
                request.TemplateParameter
            };

            string jsonString = JsonSerializer.Serialize(requestParam);
            httpRequest.Content = new StringContent(jsonString, Encoding.UTF8, "application/json");

            var response = await client.SendAsync(httpRequest);
            return true;
        }

        public async Task<object> Send(string recipient, string templateCode, object body) {
            try {
                var serialize = JsonSerializer.Serialize(body);
                var notif = new RequestNotificationSendEmailVM {
                    Recipient = recipient,
                    TemplateCode = templateCode,
                    ClientId = "SSCGI_WEBSITE",
                    TemplateParameter = serialize
                };
                return await NotificationSendEmail(notif);
            } catch (Exception e) {
                return e.Message;
            }
        }
    }
}
