using AsyJob.Jobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyJobTests.Jobs.Test_Doubles
{
    internal class ErrorJob : Job
    {
        public ErrorJob(string id, string description = "") : base(id, description)
        {
        }

        public ErrorJob(string id, string name, string description = "") : base(id, name, description)
        {
        }

        protected override void OnRun()
        {
            throw new TestException("This job always throws an exception");
        }
    }
}
