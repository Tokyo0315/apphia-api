namespace Apphia_Website_API.Repository.Model.ReadOnlySPModel
{
    public class EmailRecipientSP : SPBaseModel
    {
        public required string Email { get; set; }
        public required string Segment { get; set; }
    }
}
