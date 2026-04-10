namespace Apphia_Website_API.Repository.ViewModel.Transaction
{
    public class GalleryAlbumCreateViewModel
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int? SortOrder { get; set; }
    }

    public class GalleryAlbumUpdateViewModel : GalleryAlbumCreateViewModel { }

    public class GalleryAlbumReadViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Thumbnail { get; set; }
        public int? SortOrder { get; set; }
        public bool? IsActive { get; set; }
        public int TotalCount { get; set; }
        public string? DeletedBy { get; set; }
        public string? DeletedDate { get; set; }
    }
}
