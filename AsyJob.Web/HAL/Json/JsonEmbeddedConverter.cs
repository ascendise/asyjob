using MongoDB.Bson;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System.Dynamic;

namespace AsyJob.Web.HAL.Json
{
    public class JsonEmbeddedConverter : CustomCreationConverter<Embedded>
    {
        public override bool CanRead => false;
        public override bool CanWrite => true;

        public override Embedded Create(Type objectType)
            => [];

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            if (value is not Embedded embedded || !embedded.Any())
            {
                writer.WriteValue(null as object);
                return;
            }
            writer.WriteStartObject();
            foreach (var embed in embedded)
            {
                writer.WritePropertyName(embed.ResourceName);
                serializer.Serialize(writer, embed.Document);
            }
            writer.WriteEndObject();
        }
    }
}
