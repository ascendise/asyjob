using AsyJob.Lib.Auth;
using AsyJob.Lib.Jobs;

namespace AsyJob.Lib.Runner
{

    public class JobRunner(IJobPool pool, IAuthorizationManager authManager) : IJobRunner
    {
        /// <summary>
        /// Sets the current <see cref="User"/> of the JobRunner
        /// If User is null, then the JobRunner is ran as anonymous user with no rights.
        /// If the User does not have the appropriate rights for a method, then the
        /// method throws a <see cref="UnauthorizedException"/>
        /// </summary>
        public User? User { get; set; }
        private readonly IJobPool _pool = pool;
        private readonly IAuthorizationManager _authManager = authManager;

        public void RunJob(Job job)
        {
            _authManager.AuthenticatedContext(
                () => _pool.RunJob(job),
                User, [new Right(nameof(JobRunner), Operation.Write | Operation.Execute)]);
        }

        public Task<IEnumerable<Job>> GetJobs()
        {
            return _pool.FetchAll<Job>();
        }

        public Task<Job?> GetJob(string jobId)
        {
            return _authManager.AuthenticatedContext(
                () => _pool.FetchJob<Job>(jobId),
                User, [new Right(nameof(JobRunner), Operation.Read)]);
        }
    }
}
