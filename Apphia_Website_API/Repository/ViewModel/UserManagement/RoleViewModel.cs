namespace Apphia_Website_API.Repository.ViewModel.UserManagement
{
    public class RoleCreateViewModel
    {
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public List<PolicyCreateViewModel> PolicyControls { get; set; } = new();
    }

    public class RoleReadViewModel
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool? IsActive { get; set; }
        public List<PolicyReadViewModel> PolicyControls { get; set; } = new();
    }

    public class RoleUpdateViewModel
    {
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public List<PolicyUpdateViewModel> PolicyControls { get; set; } = new();
    }
}
// PolicyViewModel → PolicyViewModel.cs
// ControlViewModel → ControlViewModel.cs
// RolePolicyControlViewModel → RolePolicyControlViewModel.cs
// AuthenticateResponse → AuthenticationRequest.cs
