using MongoDB.Driver;
using System.Security.Cryptography;

namespace AsyJob.Web.HAL
{

    public partial class Link
    {
        private Link(string href) { Href = href; }

        public string Href { get; private set; }
        public bool? Templated { get; private set; }
        public string? Type { get; private set; }
        public string? Deprecation { get; private set; }
        public string? Name { get; private set; }
        public string? Profile { get; private set; }
        public string? Title { get; private set; }
        public string? HrefLang { get; private set; }
    }
}