namespace Apphia_Website_API.Repository.ViewModel.Common {
    public class ResponseApiViewModel {
        public int Status { get; set; }
        public bool Success { get; set; }
        public string? Message { get; set; }
        public object? Content { get; set; }
        public int? TotalCount { get; set; }
    }
}
