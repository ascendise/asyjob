using MongoDB.Driver;
using Newtonsoft.Json.Serialization;
using System.Security.Cryptography;

namespace AsyJob.Web.HAL
{

    public partial class Link(string href, bool? templated = null, string? type = null, string? deprecation = null,
        string? name = null, string? profile = null, string? title = null, string? hrefLang = null)
    {
        public string Href { get; private set; } = href;
        public bool? Templated { get; private set; } = templated;
        public string? Type { get; private set; } = type;
        public string? Deprecation { get; private set; } = deprecation;
        public string? Name { get; private set; } = name;
        public string? Profile { get; private set; } = profile;
        public string? Title { get; private set; } = title;
        public string? HrefLang { get; private set; } = hrefLang;
    }
}