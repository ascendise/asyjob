using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyJob.IntegrationTests.Users
{
    internal record UnconfirmedUserResponse(
        Guid Id,
        string Username,
        IEnumerable<string> Rights,
        [property: JsonProperty("_links")] UnconfirmedUserLinks Links);
}
