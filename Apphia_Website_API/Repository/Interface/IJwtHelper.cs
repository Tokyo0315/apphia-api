using Apphia_Website_API.Repository.ViewModel.UserManagement;

namespace Apphia_Website_API.Repository.Interface
{
    public interface IJwtHelper
    {
        string GenerateJwtToken(string userId, RoleReadViewModel role, string email = "", string fullName = "");
        string TokenDecoder(HttpContext context);
    }
}
