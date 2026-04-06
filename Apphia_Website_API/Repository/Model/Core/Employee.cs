using System.ComponentModel.DataAnnotations;

namespace Apphia_Website_API.Repository.Model.Core {
    public class Employee : BaseModel {
        [Required]
        public string EmployeeNumber { get; set; } = string.Empty;
        [Required]
        public string LastName { get; set; } = string.Empty;
        [Required]
        public string GivenName { get; set; } = string.Empty;
        public string? MiddleName { get; set; }
        [Required]
        public string Email { get; set; } = string.Empty;

        public string GetFullName() {
            return $"{GivenName} {MiddleName} {LastName}".Trim();
        }
    }
}
