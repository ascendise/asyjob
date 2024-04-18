using AsyJob.Lib.Client.Abstract.Jobs;
using AsyJob.Lib.Jobs;
using AsyJob.Lib.Jobs.Factory;
using System.ComponentModel;
using System.Dynamic;
using System.Runtime.CompilerServices;

namespace AsyJob.Lib.Client.Jobs
{
    internal class JobMapper(JobFactory jobFactory) 
        : IMapper<JobRequestDto, Job>, IMapper<Job, JobResponseDto>
    {
        private readonly JobFactory _jobFactory = jobFactory;

        public Job Map(JobRequestDto jobReq)
        {
            if(jobReq.Input == null)
            {
                return _jobFactory.CreateJob(jobReq.JobType, jobReq.Name, jobReq.Description);
            }
            return _jobFactory.CreateJobWithInput(jobReq.JobType, jobReq.Input, jobReq.Name, jobReq.Description);
        }

        public JobResponseDto Map(Job src)
        {
            var input = TryGetSubProperty<IInput<object>, object>(src, i => i.Input);
            var output = TryGetSubProperty<IOutput<object>, object>(src, i => i.Output);
            return new(src.Id, src.Name, src.Description, Enum.GetName(src.Status)!, input, output, src.Error);
        }

        private static dynamic TryGetSubProperty<TSub, TProp>(Job job, Func<TSub, TProp?> predicate)
            where TSub : class
            where TProp : class
        {
            if (job is not TSub subObject)
                return null;
            var subProperty = predicate(subObject);
            return subProperty;
        }

    }
}
