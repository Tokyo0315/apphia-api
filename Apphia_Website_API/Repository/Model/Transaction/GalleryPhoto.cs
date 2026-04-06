using System.ComponentModel.DataAnnotations;

namespace Apphia_Website_API.Repository.Model.Transaction {
    public class GalleryPhoto : BaseModel {
        [Required]
        public string Image { get; set; } = string.Empty;
        public string? Caption { get; set; }
        public int? SortOrder { get; set; }
        public int? GalleryAlbumId { get; set; }
        public GalleryAlbum? GalleryAlbum { get; set; }
    }
}
