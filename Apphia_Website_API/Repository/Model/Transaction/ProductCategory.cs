using System.ComponentModel.DataAnnotations;

namespace Apphia_Website_API.Repository.Model.Transaction {
    public class ProductCategory : BaseModel {
        [Required]
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int? SortOrder { get; set; }

        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
