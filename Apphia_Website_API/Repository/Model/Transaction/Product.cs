using System.ComponentModel.DataAnnotations;

namespace Apphia_Website_API.Repository.Model.Transaction {
    public class Product : BaseModel {
        [Required]
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Image { get; set; }
        [Required]
        public int ProductCategoryId { get; set; }
        public ProductCategory? ProductCategory { get; set; }
    }
}
