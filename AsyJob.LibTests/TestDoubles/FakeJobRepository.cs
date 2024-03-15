using AsyJob.Lib.Jobs;
using AsyJob.Lib.Runner;
using System.Reflection.Metadata.Ecma335;

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

        public Task<Job> UpdateJob(Job job)
        {
            var jobToUpdate = _jobs.FirstOrDefault(j => j.Id == job.Id)
                ?? throw new KeyNotFoundException();
            jobToUpdate.Update(job);
            return Task.FromResult(jobToUpdate);
        }

        public Task DeleteJob(string jobId)
        {
            var jobToDelete = _jobs.FirstOrDefault(j => j.Id == jobId);
            if (jobToDelete != null)
            {
                _jobs.Remove(jobToDelete);
            }
            return Task.CompletedTask;
        }
    }
}
