using MongoDB.Bson.Serialization.Serializers;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace AsyJob.Web.HAL.Json
{
    public class JsonLinksConverter : CustomCreationConverter<Links>
    {
        public override bool CanRead => false;
        public override bool CanWrite => true;

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Links);
        }

        public override Links Create(Type objectType)
            => [];

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            if (value is not Links links || !links.Any())
            {
                writer.WriteValue(null as object);
                return;
            }
            writer.WriteStartObject();
            foreach (var link in links)
            {
                writer.WritePropertyName(link.Key);
                serializer.Serialize(writer, link.Value);
            }
            writer.WriteEndObject();
        }

    }
}
