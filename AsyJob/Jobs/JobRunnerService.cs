namespace AsyJob.Jobs
{
    public interface IJobRunner
    {
        void RunJob(Job job);
    }

    public class JobRunnerService(IJobPool pool) : IJobRunner
    {
        private readonly IJobPool _pool = pool;

        public void RunJob(Job job)
        {
            _pool.RunJob(job);
        }
    }
}
