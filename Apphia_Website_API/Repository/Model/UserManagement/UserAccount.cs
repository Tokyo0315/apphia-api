using System.ComponentModel.DataAnnotations;
using Apphia_Website_API.Repository.Model.Core;

namespace Apphia_Website_API.Repository.Model.UserManagement {
    public class UserAccount : BaseModel {
        [Required]
        public string UserID { get; set; } = string.Empty;
        public string PasswordSalt { get; set; } = string.Empty;
        public string Salt { get; set; } = string.Empty;
        [Required]
        public DateTime ExpiryDate { get; set; }
        public bool? IsLocked { get; set; }
        public bool? RequiredPasswordChange { get; set; }
        public int FailedAttempt { get; set; }
        public DateTime? LockedDate { get; set; }
        [Required]
        public int EmployeeId { get; set; }
        [Required]
        public int RoleId { get; set; }

        // Navigation properties
        public Employee? Employee { get; set; }
        public Role? Role { get; set; }
    }
}
