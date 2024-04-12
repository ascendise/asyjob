using Newtonsoft.Json;

namespace AsyJob.Web.HAL.Json
{
    public static class JsonHal
    {
        public static IList<JsonConverter> Converters
        {
            get => [
                new JsonLinksConverter(),
                new JsonEmbeddedConverter()
            ];
        }
    }
}
