using System.ComponentModel.DataAnnotations;

namespace Apphia_Website_API.Repository.Model.Core {
    public class PasswordResetRequest : BaseModel {
        [Required]
        public int UserAccountId { get; set; }
        [Required]
        public string Token { get; set; } = string.Empty;
        public DateTime ExpirationDate { get; set; }
        public bool IsUsed { get; set; }
    }
}
