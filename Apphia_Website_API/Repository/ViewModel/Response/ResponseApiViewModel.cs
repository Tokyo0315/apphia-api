using System.Text.Json.Serialization;

namespace Apphia_Website_API
{
    public class ResponseApiViewModel
    {
        public int Status { get; set; }
        public bool Success { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Message { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public object? Content { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? TotalCount { get; set; }
    }
}
