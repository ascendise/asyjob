using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace AsyJob.Web.HAL.Json
{
    public class EmbeddedConverter : CustomCreationConverter<Embedded>
    {
        public override Embedded Create(Type objectType)
            => [];

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            if (value is not Embedded embedded)
                return;
            writer.WriteStartObject();
            var counter = 1;
            foreach(var embed in embedded)
            {
                writer.WritePropertyName(embed.Name ?? (counter++).ToString());
                serializer.Serialize(writer, embed.Document);
            }
        }
    }
}
