namespace Apphia_Website_API.Repository.Model.ReadOnlySPModel
{
    public class GalleryPhotoSP : SPBaseModel
    {
        public string? Image { get; set; }
        public string? Caption { get; set; }
        public string? Album { get; set; }
        public int? SortOrder { get; set; }
    }
}
