
namespace Apphia_Website_API.Repository.ViewModel.Core {
    public class EmployeeReadViewModel {
        public string EmployeeNumber { get; set; } = string.Empty;
        public string GivenName { get; set; } = string.Empty;
        public string? MiddleName { get; set; }
        public string LastName { get; set; } = string.Empty;
    }
    public class EmployeeUpdateViewModel {
        public string LastName { get; set; } = string.Empty;
        public string GivenName { get; set; } = string.Empty;
        public string? MiddleName { get; set; }
    }
    public class DashboardLatestGeneratedDateViewModel {
        public DateTime? GeneratedDate { get; set; }
    }
    public class DashboardChartViewModel {
        public string Name { get; set; } = string.Empty;
        public int Value { get; set; }
    }
    public class DashboardCreateViewModel {
        public string SecretCode { get; set; } = string.Empty;
        public string Domain { get; set; } = string.Empty;
        public object? Model { get; set; }
    }
    public class ApprovalProcessViewModel {
        public bool IsApprove { get; set; }
        public string? Reason { get; set; }
    }
    public class ApprovalViewModel {
        public string? Token { get; set; }
        public string Module { get; set; } = string.Empty;
        public int TransactionId { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string? Title { get; set; }
        public string? EmailAddress { get; set; }
        public object? Content { get; set; }
    }
}
