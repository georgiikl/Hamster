using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Hamster.Adapters.Implementation.AlphaVantage
{
    internal class NullableLongConverter : JsonConverter<long?>
    {
        public override long? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var s = reader.GetString();
            if (s is null or "None") return null;
            return long.TryParse(s, out var x) ? x : null;
        }

        public override void Write(Utf8JsonWriter writer, long? value, JsonSerializerOptions options)
        {
            var s = value?.ToString();
            writer.WriteStringValue(s);
        }
    }
}