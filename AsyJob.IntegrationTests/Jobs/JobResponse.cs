using AsyJob.IntegrationTests.Users;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace AsyJob.IntegrationTests.Jobs
{
    internal record JobResponse(
        string Id,
        string Name,
        string Description,
        string ProgressStatus,
        [property: JsonProperty("_links")] JobLinks Links,
        IDictionary<string, object?>? Output = null,
        IDictionary<string, object?>? Input = null,
        Exception? Error = null);

    internal record JobLinks(
        Link Self,
        Link Jobs);
}
