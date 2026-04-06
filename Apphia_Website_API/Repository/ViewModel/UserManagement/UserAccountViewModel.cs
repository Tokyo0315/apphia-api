namespace Apphia_Website_API.Repository.ViewModel.UserManagement {
    public class UserAccountCreateViewModel {
        public string Email { get; set; } = string.Empty;
        public string EmployeeNumber { get; set; } = string.Empty;
        public string GivenName { get; set; } = string.Empty;
        public string? MiddleName { get; set; }
        public string LastName { get; set; } = string.Empty;
        public int RoleId { get; set; }
    }
    public class UserAccountReadViewModel {
        public int Id { get; set; }
        public string UserID { get; set; } = string.Empty;
        public string EmployeeNumber { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string GivenName { get; set; } = string.Empty;
        public string? MiddleName { get; set; }
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int RoleId { get; set; }
        public string Role { get; set; } = string.Empty;
        public string ExpiryDate { get; set; } = string.Empty;
        public string? IsLocked { get; set; }
        public bool? IsActive { get; set; }
        public int TotalCount { get; set; }
        public string? DeletedBy { get; set; }
        public string? DeletedDate { get; set; }
    }
    public class UserAccountUpdateViewModel : UserAccountCreateViewModel {
        public string? Password { get; set; }
    }
    public class UserAccountChangePassword {
        public string Token { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
    public class LoginRequestViewModel {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
