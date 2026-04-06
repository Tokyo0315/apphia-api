using System.ComponentModel.DataAnnotations;

namespace Apphia_Website_API.Repository.Model.Core {
    public class DashboardPage {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Domain { get; set; } = string.Empty;
        [Required]
        public string Url { get; set; } = string.Empty;
        [Required]
        public int Count { get; set; }
        public DateTime GeneratedDate { get; set; }
    }

    public class DashboardCountry {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Domain { get; set; } = string.Empty;
        [Required]
        public string Country { get; set; } = string.Empty;
        [Required]
        public int Count { get; set; }
        public DateTime GeneratedDate { get; set; }
    }

    public class DashboardDevice {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Domain { get; set; } = string.Empty;
        [Required]
        public string Device { get; set; } = string.Empty;
        [Required]
        public int Count { get; set; }
        public DateTime GeneratedDate { get; set; }
    }

    public class DashboardEngagement {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Domain { get; set; } = string.Empty;
        [Required]
        public int TotalTime { get; set; }
        [Required]
        public int ActiveTime { get; set; }
        public DateTime GeneratedDate { get; set; }
    }
}
