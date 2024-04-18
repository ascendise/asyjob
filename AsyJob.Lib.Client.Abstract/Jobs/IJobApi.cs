namespace AsyJob.Lib.Client.Abstract.Jobs
{
    public interface IJobApi
    {
        Task<JobResponseDto> CreateJob(JobRequestDto job);
        Task<JobResponseDto> FetchJob(string jobId);
        Task<IEnumerable<JobResponseDto>> FetchAllJobs();
    }
}
