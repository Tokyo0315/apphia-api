using System.ComponentModel.DataAnnotations;

namespace Apphia_Website_API.Repository.Model
{
    public class SPBaseModel
    {
        [Key]
        public int Id { get; set; }
        public string? CreatedBy { get; set; }
        public string? CreatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public string? UpdatedDate { get; set; }
        public string? DeletedBy { get; set; }
        public string? DeletedDate { get; set; }
        public string? RestoredBy { get; set; }
        public string? RestoredDate { get; set; }
        public int? TotalCount { get; set; }
    }
}
