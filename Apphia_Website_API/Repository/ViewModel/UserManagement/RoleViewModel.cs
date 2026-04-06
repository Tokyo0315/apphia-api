namespace Apphia_Website_API.Repository.ViewModel.UserManagement {
    public class RoleCreateViewModel {
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public List<PolicyCreateViewModel> PolicyControls { get; set; } = new();
    }
    public class RoleReadViewModel {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool? IsActive { get; set; }
        public List<PolicyReadViewModel> PolicyControls { get; set; } = new();
    }
    public class RoleUpdateViewModel {
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public List<PolicyUpdateViewModel> PolicyControls { get; set; } = new();
    }
    public class PolicyCreateViewModel {
        public string Name { get; set; } = string.Empty;
        public ControlCreateViewModel Controls { get; set; } = new();
    }
    public class PolicyReadViewModel {
        public int Id { get; set; }
        public string? Name { get; set; }
        public ControlReadViewModel Controls { get; set; } = new();
    }
    public class PolicyUpdateViewModel {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public ControlUpdateViewModel Controls { get; set; } = new();
    }
    public class ControlCreateViewModel {
        public bool Create { get; set; }
        public bool Read { get; set; }
        public bool Update { get; set; }
        public bool Delete { get; set; }
        public bool Restore { get; set; }
    }
    public class ControlReadViewModel {
        public int Id { get; set; }
        public bool Create { get; set; }
        public bool Read { get; set; }
        public bool Update { get; set; }
        public bool Delete { get; set; }
        public bool Restore { get; set; }
    }
    public class ControlUpdateViewModel {
        public bool Create { get; set; }
        public bool Read { get; set; }
        public bool Update { get; set; }
        public bool Delete { get; set; }
        public bool Restore { get; set; }
    }
    public class RolePolicyControlCreateViewModel {
        public int RoleId { get; set; }
        public int PolicyId { get; set; }
        public int ControlId { get; set; }
    }
    public class RolePolicyControlReadViewModel {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public int PolicyId { get; set; }
        public int ControlId { get; set; }
    }
    public class AuthenticateResponse {
        public int userId { get; set; }
        public string Token { get; set; } = string.Empty;
        public string UserFirstname { get; set; } = string.Empty;
        public string UserLastname { get; set; } = string.Empty;
        public string RoleName { get; set; } = string.Empty;
        public int EmployeeId { get; set; }
        public int roleId { get; set; }
    }
}
