namespace Apphia_Website_API.Repository.ViewModel.Transaction {
    public class ContactUsViewModel {
        public string? firstName { get; set; }
        public string? lastName { get; set; }
        public string? email { get; set; }
        public string? contactNo { get; set; }
        public string? inquiry { get; set; }
        public string? message { get; set; }
        public bool cookieAccepted { get; set; }
    }
    // NEW Apphia ViewModels
    public class ProductCreateViewModel {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int ProductCategoryId { get; set; }
    }
    public class ProductUpdateViewModel : ProductCreateViewModel { }
    public class ProductCategoryCreateViewModel {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int? SortOrder { get; set; }
    }
    public class ProductCategoryUpdateViewModel : ProductCategoryCreateViewModel { }
    public class GalleryAlbumCreateViewModel {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int? SortOrder { get; set; }
    }
    public class GalleryAlbumUpdateViewModel : GalleryAlbumCreateViewModel { }
    public class GalleryPhotoCreateViewModel {
        public string? Caption { get; set; }
        public int? SortOrder { get; set; }
        public int? GalleryAlbumId { get; set; }
    }
    public class GalleryPhotoUpdateViewModel : GalleryPhotoCreateViewModel { }

    // =============== READ VIEW MODELS =============== //
    public class ContactReadViewModel {
        public int Id { get; set; }
        public string Inquiry { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string ContactNo { get; set; } = string.Empty;
        public bool? IsActive { get; set; }
        public int TotalCount { get; set; }
        public string? DeletedBy { get; set; }
        public string? DeletedDate { get; set; }
    }
    public class ProductReadViewModel {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string Category { get; set; } = string.Empty;
        public bool? IsActive { get; set; }
        public int TotalCount { get; set; }
        public string? DeletedBy { get; set; }
        public string? DeletedDate { get; set; }
    }
    public class GalleryPhotoReadViewModel {
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
