namespace AsyJob.Jobs
{
    public interface IJobRunner
    {
        void RunJob(Job job);
    }

    public class JobRunnerService : IJobRunner
    {
        public void RunJob(Job job)
        {
            var jobThread = new Thread(() => job.Run());
            jobThread.Start();
        }
    }
}
