using AsyJob.Lib.Jobs;

namespace AsyJobTests.Jobs
{
    internal static class JobTestUtils
    {
        public static void WaitForJobCompletion(Job job)
        {
            while (job.Status != ProgressStatus.Done && job.Status != ProgressStatus.Error)
            {
                //Loop until finished
            }
        }
    }

}
