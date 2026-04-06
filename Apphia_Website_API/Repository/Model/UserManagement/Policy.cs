using System.ComponentModel.DataAnnotations;

namespace Apphia_Website_API.Repository.Model.UserManagement {
    public class Policy : BaseModel {
        [Required]
        public string Name { get; set; } = string.Empty;
    }
}
