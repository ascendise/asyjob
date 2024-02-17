using AsyJob.Jobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyJobTests.Jobs
{
    internal static class JobTestUtils
    {
        public static void WaitForJobCompletion(Job job)
        {
            while(job.Status != ProgressStatus.Done && job.Status != ProgressStatus.Error)
            {
                //Loop until finished
            }
        }
    }
}
