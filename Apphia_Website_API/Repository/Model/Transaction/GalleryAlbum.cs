using System.ComponentModel.DataAnnotations;

namespace Apphia_Website_API.Repository.Model.Transaction {
    public class GalleryAlbum : BaseModel {
        [Required]
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Thumbnail { get; set; }
        public int? SortOrder { get; set; }

        public ICollection<GalleryPhoto> Photos { get; set; } = new List<GalleryPhoto>();
    }
}
