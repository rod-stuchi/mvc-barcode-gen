using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDResearch.Core.Converters
{
    public class ConverterInt32 : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(Int32));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.String)
            {
                if (string.IsNullOrEmpty((string)reader.Value))
                    return 0;

                int num;
                if (int.TryParse((string)reader.Value, out num))
                    return num;
            }

            return 0;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(value);
        }
    }

}
