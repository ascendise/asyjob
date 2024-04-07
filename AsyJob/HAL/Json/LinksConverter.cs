using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace AsyJob.Web.HAL.Json
{
    public class LinksConverter : CustomCreationConverter<Links>
    {
        public override Links Create(Type objectType)
            => [];

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            if (value is not Links links)
                return;
            writer.WriteStartObject();
            var counter = 1;
            foreach(var link in links)
            {
                writer.WritePropertyName(link.Name ?? (counter++).ToString());
                serializer.Serialize(writer, link);
            }
            writer.WriteEndObject();
        }
    }
}
