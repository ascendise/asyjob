using AsyJob.Lib.Jobs;

namespace AsyJob.Lib.Runner
{

    /// <summary>
    /// Interface for specific job factories
    /// The job factory is responsible for making one specific type of job that matches the <see cref="JobType"/>.
    /// </summary>
    public interface IJobFactory
    {
        public string JobType { get; }

        /// <summary>
        /// Create a new <see cref="Job"/> with the specified type
        /// </summary>
        Job CreateJob(string type, string id, string name = "", string description = "");
    }

    /// <summary>
    /// Interface for specific job factories
    /// The job factory is responsible for making one specific type of job that matches the <see cref="JobType"/>.
    /// </summary>
    public interface IJobWithInputFactory
    {
        public string JobType { get; }

        /// <summary>
        /// Create a new <see cref="Job"/> with the specified type
        /// </summary>
        /// <exception cref="JobInputMismatchException">Thrown when the input does not match the expected structure</exception>
        Job CreateJobWithInput(string type, string id, dynamic input, string name = "", string description = "");
    }

    public class JobFactory(
        IEnumerable<IJobFactory>? jobFactories,
        IEnumerable<IJobWithInputFactory>? jobWithInputFactories,
        IGuidProvider guidProvider
    )
    {
        public string JobType => "Job";
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
