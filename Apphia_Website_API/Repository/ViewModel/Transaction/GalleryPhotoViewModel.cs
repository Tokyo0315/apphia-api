namespace Apphia_Website_API.Repository.ViewModel.Transaction
{
    public class GalleryPhotoCreateViewModel
    {
        public string? Caption { get; set; }
        public int? SortOrder { get; set; }
        public int? GalleryAlbumId { get; set; }
    }

    public class GalleryPhotoUpdateViewModel : GalleryPhotoCreateViewModel { }

    public class GalleryPhotoReadViewModel
    {
        public int Id { get; set; }
        public string? Caption { get; set; }
        public string Album { get; set; } = string.Empty;
        public int? SortOrder { get; set; }
        public bool? IsActive { get; set; }
        public int TotalCount { get; set; }
        public string? DeletedBy { get; set; }
        public string? DeletedDate { get; set; }
    }

}
