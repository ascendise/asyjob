using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyJob.IntegrationTests.Users
{
    internal class UnconfirmedUserResponse
    {
        public Guid Id { get; set; }
        public string? Username { get; set; }
        public IEnumerable<string>? Rights { get; set; }
        [JsonProperty("_links")]
        public UnconfirmedUserLinks? Links { get; set; }
    }

    internal class UnconfirmedUserLinks
    {
        public Link? Users { get; set; }
        public Link? Self { get; set; }
        public Link? Confirm { get; set; }
    }

    internal class Link
    {
        public string? Href { get; set; }
    }
}
