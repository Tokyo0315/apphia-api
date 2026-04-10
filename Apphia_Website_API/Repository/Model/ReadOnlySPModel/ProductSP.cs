namespace Apphia_Website_API.Repository.Model.ReadOnlySPModel
{
    public class ProductSP : SPBaseModel
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Category { get; set; }
        public string? Image { get; set; }
    }
}
