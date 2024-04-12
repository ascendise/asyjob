using Newtonsoft.Json;

namespace AsyJob.Web.HAL
{
    public readonly struct Embed(string resourceName, HalDocument document)
    {
        public string ResourceName { get; } = resourceName;
        public HalDocument Document { get; } = document;
    }
}
