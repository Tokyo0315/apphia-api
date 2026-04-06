using System.ComponentModel.DataAnnotations;

namespace Apphia_Website_API.Repository.Model.UserManagement {
    public class TempPassword : BaseModel {
        [Required]
        public string tempPassword { get; set; } = string.Empty;
        [Required]
        public int UserID { get; set; }
    }
}
