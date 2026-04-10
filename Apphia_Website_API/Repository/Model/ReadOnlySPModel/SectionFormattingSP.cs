namespace Apphia_Website_API.Repository.Model.ReadOnlySPModel
{
    public class SectionFormattingSP : SPBaseModel
    {
        public string? Section { get; set; }
        public string? Description { get; set; }
        public int? SequenceOrder { get; set; }
        public string? ApprovalStatus { get; set; }
    }
}
