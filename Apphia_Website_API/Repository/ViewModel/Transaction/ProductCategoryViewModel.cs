namespace Apphia_Website_API.Repository.ViewModel.Transaction
{
    public class ProductCategoryCreateViewModel
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int? SortOrder { get; set; }
    }

    public class ProductCategoryUpdateViewModel : ProductCategoryCreateViewModel { }

    public class ProductCategoryReadViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int? SortOrder { get; set; }
        public bool? IsActive { get; set; }
        public int TotalCount { get; set; }
        public string? DeletedBy { get; set; }
        public string? DeletedDate { get; set; }
    }
}
