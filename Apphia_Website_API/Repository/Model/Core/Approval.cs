using System.ComponentModel.DataAnnotations;

namespace Apphia_Website_API.Repository.Model.Core {
    public class Approval : BaseModel {
        public string Token { get; set; } = Guid.NewGuid().ToString("N");
        [Required]
        public string Module { get; set; } = string.Empty;
        [Required]
        public int TransactionId { get; set; }
        public DateTime ExpirationDate { get; set; } = DateTime.UtcNow.AddDays(2);
    }
}
