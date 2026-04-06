using System.ComponentModel.DataAnnotations;

namespace Apphia_Website_API.Repository.Model.UserManagement {
    public class RolePolicyControl : BaseModel {
        [Required]
        public int RoleId { get; set; }
        [Required]
        public int PolicyId { get; set; }
        [Required]
        public int ControlId { get; set; }

        // Navigation properties
        public Role? Role { get; set; }
        public Policy? Policy { get; set; }
        public Control? Control { get; set; }
    }
}
