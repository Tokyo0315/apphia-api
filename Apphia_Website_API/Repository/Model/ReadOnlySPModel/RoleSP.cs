namespace Apphia_Website_API.Repository.Model.ReadOnlySPModel
{
    public class RoleSP : SPBaseModel
    {
        public required string Code { get; set; }
        public required string Name { get; set; }
        public required string? Description { get; set; }
    }
}
