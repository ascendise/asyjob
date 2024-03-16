using AsyJob.Lib.Jobs;

namespace AsyJob.Lib.Tests.Jobs
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

        public static async Task RepeatUntil(Action repeat, Func<bool> until, int maxRetry = 10, int delayMillis = 0)
        {
            var periodicTimer = new PeriodicTimer(TimeSpan.FromMilliseconds(delayMillis));
            int tryCount = 0;
            while(tryCount < maxRetry && !until() && await periodicTimer.WaitForNextTickAsync())
            {
                repeat();
                tryCount++;
            }
        }
    }
}
