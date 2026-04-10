namespace Apphia_Website_API.Repository.ViewModel.UserManagement
{
    public class RolePolicyControlCreateViewModel
    {
        public int RoleId { get; set; }
        public int PolicyId { get; set; }
        public int ControlId { get; set; }
    }

    public class RolePolicyControlReadViewModel
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public int PolicyId { get; set; }
        public int ControlId { get; set; }
    }
}
