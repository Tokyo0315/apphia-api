namespace Apphia_Website_API.Repository.Interface
{
    public interface IRequestStatusHelper
    {
        object response(int status, bool success, string? message, object? content, int? totalCount);
    }
}
