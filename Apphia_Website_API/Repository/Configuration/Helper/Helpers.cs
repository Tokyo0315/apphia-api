using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Apphia_Website_API.Repository.Configuration.Helper {
    public class GlobalHelper {
        public static string? ConvertThumbnailToUrl(byte[]? thumbnail) {
            if (thumbnail == null || thumbnail.Length == 0) return null;
            var base64 = Convert.ToBase64String(thumbnail);
            return $"data:image/png;base64,{base64}";
        }
    }

    public class RandomHelper {
        private readonly Random _random = new Random();
        private readonly string _characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

        public string GenerateRandomString(int length) {
            StringBuilder result = new StringBuilder(length);
            for (int i = 0; i < length; i++) {
                result.Append(_characters[_random.Next(_characters.Length)]);
            }
            return result.ToString();
        }
    }

    public class LogHelper {
        public static void WriteLogToFile(string message, string destination, string endpoint, string status) {
            var dateStr = DateTime.Now.ToString("yyyyMMdd");
            var fileName = $"{dateStr}_ActionLogs.txt";
            var logEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{status}] [{endpoint}] {message}";

            if (!Directory.Exists(destination)) Directory.CreateDirectory(destination);
            var filePath = Path.Combine(destination, fileName);
            using var stream = new FileStream(filePath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
            using var writer = new StreamWriter(stream);
            writer.WriteLine(logEntry);
        }
    }

    public class DateOnlyJsonConverter : JsonConverter<DateOnly> {
        private const string Format = "yyyy-MM-dd";
        public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
            if (reader.TokenType == JsonTokenType.String && DateOnly.TryParse(reader.GetString(), out var date)) return date;
            throw new JsonException($"Unable to convert to {nameof(DateOnly)}");
        }
        public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options) {
            writer.WriteStringValue(value.ToString(Format));
        }
    }

    public class NullableDateOnlyJsonConverter : JsonConverter<DateOnly?> {
        private const string Format = "yyyy-MM-dd";
        public override DateOnly? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
            if (reader.TokenType == JsonTokenType.Null) return null;
            if (reader.TokenType == JsonTokenType.String) {
                var str = reader.GetString();
                if (string.IsNullOrWhiteSpace(str)) return null;
                if (DateOnly.TryParse(str, out var date)) return date;
            }
            throw new JsonException($"Unable to convert to {nameof(DateOnly)}");
        }
        public override void Write(Utf8JsonWriter writer, DateOnly? value, JsonSerializerOptions options) {
            if (value.HasValue) writer.WriteStringValue(value.Value.ToString(Format));
            else writer.WriteNullValue();
        }
    }
}
