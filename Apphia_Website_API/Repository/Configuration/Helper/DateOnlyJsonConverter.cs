using System.Text.Json;
using System.Text.Json.Serialization;

namespace Apphia_Website_API.Repository.Configuration.Helper
{
    public class DateOnlyJsonConverter : JsonConverter<DateOnly>
    {
        private const string Format = "yyyy-MM-dd";
        public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String && DateOnly.TryParse(reader.GetString(), out var date)) return date;
            throw new JsonException($"Unable to convert to {nameof(DateOnly)}");
        }
        public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString(Format));
        }
    }

    public class NullableDateOnlyJsonConverter : JsonConverter<DateOnly?>
    {
        private const string Format = "yyyy-MM-dd";
        public override DateOnly? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null) return null;
            if (reader.TokenType == JsonTokenType.String)
            {
                var str = reader.GetString();
                if (string.IsNullOrWhiteSpace(str)) return null;
                if (DateOnly.TryParse(str, out var date)) return date;
            }
            throw new JsonException($"Unable to convert to {nameof(DateOnly)}");
        }
        public override void Write(Utf8JsonWriter writer, DateOnly? value, JsonSerializerOptions options)
        {
            if (value.HasValue) writer.WriteStringValue(value.Value.ToString(Format));
            else writer.WriteNullValue();
        }
    }
}
