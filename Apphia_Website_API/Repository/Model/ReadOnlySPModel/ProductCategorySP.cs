namespace Apphia_Website_API.Repository.Model.ReadOnlySPModel
{
    public class ProductCategorySP : SPBaseModel
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int? SortOrder { get; set; }
    }
}
