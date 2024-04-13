using Newtonsoft.Json;

namespace AsyJob.Web.HAL
{
    public abstract class HalDocument
    {
        [JsonProperty("_links")]
        public Links Links { get; set; } = [];
        [JsonProperty("_embedded")]
        public Embedded Embedded { get; set; } = [];
    }
}
