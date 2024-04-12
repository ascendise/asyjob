using AsyJob.Lib.Jobs;

namespace AsyJob.Web.Jobs
{

    public class JobResponseDto(Job job)
    {
        public Job Job { get; private set; } = job;
    }
}
