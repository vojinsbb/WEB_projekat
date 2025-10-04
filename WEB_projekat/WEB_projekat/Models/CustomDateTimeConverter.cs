using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace WEB_projekat.Models
{
    public class CustomDateTimeConverter : JsonConverter<DateTime>
    {
        private const string Format = "dd/MM/yyyy HH:mm";

        public override void WriteJson(JsonWriter writer, DateTime value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString(Format, CultureInfo.InvariantCulture));
        }

        public override DateTime ReadJson(JsonReader reader, Type objectType, DateTime existingValue,
    bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.Value == null)
            {
                return existingValue;
            }

            if (reader.Value is DateTime dt)
            {
                return dt; // već je DateTime
            }

            if (reader.Value is string s)
            {
                return DateTime.ParseExact(s, Format, CultureInfo.InvariantCulture);
            }

            throw new JsonSerializationException($"Unexpected token type: {reader.Value.GetType()}");
        }

    }
}