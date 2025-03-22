using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SnapSell.Infrastructure.JsonSerilizeServices
{
    public class DateTimeFormatService : JsonConverter<DateTime>
    {
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string dateTimeString = reader.GetString()!;

            DateTimeOffset dateTimeOffset = DateTimeOffset.Parse(dateTimeString, null, DateTimeStyles.AssumeUniversal);

            return dateTimeOffset.UtcDateTime;
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString("yyyy-MM-ddTHH:mm:ssZ"));
        }
    }
}
