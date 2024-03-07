using AsyJob.Lib.Jobs;

namespace AsyJob.Lib.Runner
{

    public class JobFactory(
        IEnumerable<IJobFactory>? jobFactories,
        IEnumerable<IJobWithInputFactory>? jobWithInputFactories,
        IGuidProvider guidProvider
    )
    {
        public static string JobType => "Job";
        private readonly IEnumerable<IJobFactory> _jobFactories = jobFactories ?? [];
        private readonly IEnumerable<IJobWithInputFactory> _jobWithInputFactories = jobWithInputFactories ?? [];
        private readonly IGuidProvider _guidProvider = guidProvider;

        public Job CreateJob(string type, string name = "", string description = "")
        {
            var id = GetJobId();
            name = GetNameOrDefault(name, type, id);
            var factory = _jobFactories.FirstOrDefault(f => f.JobType.Equals(type, StringComparison.OrdinalIgnoreCase));
            return factory == null ? throw new NoMatchingJobFactoryException(type) : factory.CreateJob(type, id, name, description);
        }

        private string GetJobId()
            => _guidProvider.GetGuid().ToString().ToUpper();

        private static string GetNameOrDefault(string name, string type, string id)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return GenerateJobName(type, id);
            }
            return name;
        }

        private static string GenerateJobName(string type, string id)
            => $"{type}-{id}".ToUpper();

        public Job CreateJobWithInput(string type, dynamic input, string name = "", string description = "")
        {
            var id = GetJobId();
            name = GetNameOrDefault(name, type, id);
            var factory = _jobWithInputFactories.FirstOrDefault(f => f.JobType.Equals(type, StringComparison.OrdinalIgnoreCase));
            return factory == null ? throw new NoMatchingJobFactoryException(type) : factory.CreateJobWithInput(type, id, input, name, description);
        }
    }
}
