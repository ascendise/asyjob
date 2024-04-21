namespace AsyJob.Lib.Client.Abstract.Jobs
{
    public interface IJobApi
    {
        Task<JobResponse> RunJob(JobRequest job);
        Task<JobResponse> FetchJob(string jobId);
        Task<IEnumerable<JobResponse>> FetchAllJobs();
    }
}
