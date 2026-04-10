namespace Apphia_Website_API.Repository.Model.ReadOnlySPModel
{
    public class ContactSP : SPBaseModel
    {
        public string? inquiry { get; set; }
        public string? Name { get; set; }
        public string? email { get; set; }
        public string? contactNo { get; set; }
        public string? message { get; set; }
    }
}
