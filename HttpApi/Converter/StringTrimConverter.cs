using Newtonsoft.Json;

namespace HttpApi.Converter
{
    public class StringTrimConverter : JsonConverter<string?>
    {
        public override void WriteJson(JsonWriter writer, string? value, JsonSerializer serializer)
        {
            if (value is null) writer.WriteNull();
            else writer.WriteValue(value.Trim());
        }

        public override string? ReadJson(JsonReader reader, Type objectType, string? existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null || reader.Value is null) return null;
            return ((string)reader.Value).Trim();
        }
    }
}
