using System.ComponentModel.DataAnnotations;

namespace Apphia_Website_API.Repository.Model.ApiCommand {
    public class ApiCommand {
        [Key]
        public int Id { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? Value { get; set; }
        public bool? IsActive { get; set; }
    }
}
