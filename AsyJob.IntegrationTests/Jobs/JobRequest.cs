using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyJob.IntegrationTests.Jobs
{
    public class JobRequest(string jobtype, string name = "", string description = "", IDictionary<string, object?>? input = null)
    {
        public string Name { get; private set; } = name;
        public string Description { get; private set; } = description;
        public string JobType { get; private set; } = jobtype;
        public IDictionary<string, object?>? Input { get; private set; } = input;
    }
}
