using AsyJob.Jobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyJobTests.Jobs.Test_Doubles
{
    internal class DummyJob : Job
    {
        public DummyJob(string id, string description = "") : base(id, description)
        {
        }

        public DummyJob(string id, string name, string description = "") : base(id, name, description)
        {
        }
    }
}
