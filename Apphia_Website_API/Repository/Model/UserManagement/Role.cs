using System.ComponentModel.DataAnnotations;

namespace Apphia_Website_API.Repository.Model.UserManagement {
    public class Role : BaseModel {
        [Required]
        public string Code { get; set; } = string.Empty;
        [Required]
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}
