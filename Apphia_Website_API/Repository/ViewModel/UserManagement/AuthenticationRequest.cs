namespace Apphia_Website_API.Repository.ViewModel.UserManagement
{
    public class AuthenticationRequest
    {
        public required int userId { get; set; }
        public required string EmployeeNumber { get; set; }
        public required string LastName { get; set; }
        public required string GivenName { get; set; }
        public List<PolicyReadViewModel> PolicyControls { get; set; } = new();
    }

    public class AuthenticateResponse
    {
        public int userId { get; set; }
        public string Token { get; set; } = string.Empty;
        public string UserFirstname { get; set; } = string.Empty;
        public string UserLastname { get; set; } = string.Empty;
        public string RoleName { get; set; } = string.Empty;
        public int EmployeeId { get; set; }
        public int roleId { get; set; }
    }

    public class LoginAuthenticationRequest
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}
