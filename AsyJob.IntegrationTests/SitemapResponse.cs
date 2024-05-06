using AsyJob.IntegrationTests.Users;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyJob.IntegrationTests
{
    internal record SitemapResponse(
        string Message,
        [property: JsonProperty("_links")] SitemapLinks Links);

    internal record SitemapLinks(
        Link Docs, 
        Link Register, 
        Link Login,
        Link Jobs, 
        Link Users, 
        Link UnconfirmedUsers);
}

