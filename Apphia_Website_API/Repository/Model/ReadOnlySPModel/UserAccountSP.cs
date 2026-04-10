namespace Apphia_Website_API.Repository.Model.ReadOnlySPModel
{
    public class UserAccountSP : SPBaseModel
    {
        public string? EmployeeNumber { get; set; }
        public string? Name { get; set; }
        public string? Role { get; set; }
        public string? LastName { get; set; }
        public string? GivenName { get; set; }
        public string? MiddleName { get; set; }
        public string? Email { get; set; }
        public string? ExpiryDate { get; set; }
        public string? isLocked { get; set; }
    }
}
