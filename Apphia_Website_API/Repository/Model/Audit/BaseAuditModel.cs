using System.ComponentModel.DataAnnotations;

namespace Apphia_Website_API.Repository.Model.Audit {
    public abstract class BaseAuditModel {
        [Key]
        public int Id { get; set; }
        public string Action { get; set; } = string.Empty;
        public string Details { get; set; } = string.Empty;
        public string ActionByUserId { get; set; } = string.Empty;
        public string ActionByEmail { get; set; } = string.Empty;
        public string ActionByRoleId { get; set; } = string.Empty;
        public string ActionByRole { get; set; } = string.Empty;
        public string ActionByName { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }

}
