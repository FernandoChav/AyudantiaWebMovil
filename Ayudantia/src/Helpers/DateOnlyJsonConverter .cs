using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Ayudantia.Src.Helpers
{
    public class DateOnlyJsonConverter : JsonConverter<DateOnly>
    {
        private const string Format = "yyyy-MM-dd";

        public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var raw = reader.GetString();

            if (string.IsNullOrWhiteSpace(raw))
                throw new JsonException("La fecha no puede estar vacía.");

            if (!DateOnly.TryParseExact(raw, Format, out var date))
                throw new JsonException($"El formato de fecha es inválido. Usa '{Format}' (por ejemplo, 2002-08-12).");

            return date;
        }

        public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString(Format));
        }
    }
}