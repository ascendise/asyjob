using AsyJob.Lib.Client.Abstract.Jobs;
using AsyJob.Lib.Jobs;
using AsyJob.Lib.Jobs.Factory;
using System.ComponentModel;
using System.Dynamic;
using System.Runtime.CompilerServices;

namespace AsyJob.Lib.Client.Jobs
{
    internal class JobMapper(JobFactory jobFactory) 
        : IMapper<JobRequest, Job>, IMapper<Job, JobResponse>
    {
        private readonly JobFactory _jobFactory = jobFactory;

        public Job Map(JobRequest jobReq)
        {
            if(jobReq.Input == null)
            {
                return _jobFactory.CreateJob(jobReq.JobType, jobReq.Name, jobReq.Description);
            }
            return _jobFactory.CreateJobWithInput(jobReq.JobType, jobReq.Input, jobReq.Name, jobReq.Description);
        }

        public JobResponse Map(Job job)
        {
            IDictionary<string, object?>? input = null;
            if (job is IInput jobInput)
                input = jobInput.GetInputDict();
            IDictionary<string, object?>? output = null;
            if (job is IOutput jobOutput)
                output = jobOutput.GetOutputDict();
            return new(job.Id, job.Name, job.Description, Enum.GetName(job.Status)!, input, output, job.Error);
        }
    }
}
