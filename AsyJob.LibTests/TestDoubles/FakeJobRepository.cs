using AsyJob.Lib.Jobs;
using AsyJob.Lib.Runner;

namespace AsyJob.Lib.Tests.TestDoubles
{
    public class FakeJobRepository(List<Job> jobs) : IJobRepository
    {
        public IReadOnlyCollection<Job> Jobs { get => _jobs; }
        private readonly List<Job> _jobs = jobs;

        public FakeJobRepository() : this([]) { }

        public Task SaveJob(Job job)
        {
            if (_jobs.Any(j => j.Id == job.Id))
            {
                throw new DuplicateKeyException();
            }
            _jobs.Add(job);
            return Task.CompletedTask;
        }

        public Task<IEnumerable<Job>> FetchAllJobs()
        {
            return Task.FromResult(_jobs as IEnumerable<Job>);
        }

        public Task<Job?> FetchJob(string id)
        {
            return Task.FromResult(_jobs.FirstOrDefault(j => j.Id == id));
        }
    }
}
