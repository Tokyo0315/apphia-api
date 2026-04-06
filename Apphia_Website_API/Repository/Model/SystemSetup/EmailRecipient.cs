using System.ComponentModel.DataAnnotations;

namespace Apphia_Website_API.Repository.Model.SystemSetup {
    public class EmailRecipient : BaseModel {
        [Required]
        public string email { get; set; } = string.Empty;
        [Required]
        public string segment { get; set; } = string.Empty;
    }
}
