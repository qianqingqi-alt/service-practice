using Newtonsoft.Json;

namespace HttpApi.Converter
{
    public class DateTimeToTimestampConverter : JsonConverter<DateTime?>
    {
        public override void WriteJson(JsonWriter writer, DateTime? value, JsonSerializer serializer)
        {
            if (value is null)
            {
                writer.WriteNull();
            }
            // 将日期转换为时间戳（毫秒）
            long timestamp = new DateTimeOffset(value.Value).ToUnixTimeMilliseconds();
            writer.WriteValue(timestamp);
        }

        public override DateTime? ReadJson(JsonReader reader, Type objectType, DateTime? existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            // 将时间戳转换为日期（DateTime）
            if (reader.TokenType == JsonToken.Integer)
            {
                long timestamp = (long)reader.Value;
                return DateTime.SpecifyKind(DateTimeOffset.FromUnixTimeMilliseconds(timestamp).DateTime, DateTimeKind.Utc);
            }

            throw new JsonSerializationException("Invalid timestamp value.");
        }
    }
}
