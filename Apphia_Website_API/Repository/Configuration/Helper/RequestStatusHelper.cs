using Apphia_Website_API.Repository.Interface;
using Apphia_Website_API.Repository.ViewModel.Common;

namespace Apphia_Website_API.Repository.Configuration.Helper {
    public class RequestStatusHelper : IRequestStatusHelper {
        public object response(int status, bool success, string? message, object? content, int? totalCount) {
            return new ResponseApiViewModel {
                Status = status,
                Success = success,
                Message = message,
                Content = content,
                TotalCount = totalCount
            };
        }
    }
}
