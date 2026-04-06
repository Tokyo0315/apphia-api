using System.ComponentModel.DataAnnotations;

namespace Apphia_Website_API.Repository.Model {
    public class BaseModel {
        [Key]
        public int Id { get; set; }
        public bool? IsActive { get; set; }
        public int? CreatedByUserId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? UpdatedByUserId { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? DeletedByUserId { get; set; }
        public DateTime? DeletedDate { get; set; }
        public int? RestoredByUserId { get; set; }
        public DateTime? RestoredDate { get; set; }
    }

    public class SPBaseModel {
        [Key]
        public int Id { get; set; }
    }
}
