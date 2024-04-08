using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

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
                return;
            writer.WriteStartObject();
            var jObject = new JObject();
            var counter = 1;
            foreach(var embed in embedded)
            {
                var propertyName = embed.Name ?? (counter++).ToString();
                jObject.Add(propertyName, JToken.FromObject(embed.Document, serializer));
            }
            writer.WriteEndObject();
            jObject.WriteTo(writer);
        }
    }
}
