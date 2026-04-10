namespace Apphia_Website_API.Repository.ViewModel.Transaction
{
    public class ProductCreateViewModel
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int ProductCategoryId { get; set; }
    }

    public class ProductUpdateViewModel : ProductCreateViewModel { }

    public class ProductReadViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string Category { get; set; } = string.Empty;
        public bool? IsActive { get; set; }
        public int TotalCount { get; set; }
        public string? DeletedBy { get; set; }
        public string? DeletedDate { get; set; }
    }

    public class ProductWebsiteViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string Category { get; set; } = string.Empty;
        public string? Image { get; set; }
    }
}
