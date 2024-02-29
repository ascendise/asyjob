using ZstdSharp;

namespace AsyJob.Jobs
{
    public interface IJobFactory
    {
        public string JobType { get; }
        Job CreateJob(string type, string id, string name = "", string description = "");
    }

    public interface IJobWithInputFactory
    {
        public string JobType { get; }
        Job CreateJob<TInput>(string type, string id, TInput input, string name = "", string description = "");
    }

    public class JobFactory(IEnumerable<IJobFactory>? jobFactories, IEnumerable<IJobWithInputFactory>? jobWithInputFactories) : IJobFactory, IJobWithInputFactory
    {
        public string JobType => "Job";
        private readonly IEnumerable<IJobFactory> _jobFactories = jobFactories ?? [];
        private readonly IEnumerable<IJobWithInputFactory> _jobWithInputFactories = jobWithInputFactories ?? [];

        public Job CreateJob(string type, string id, string name = "", string description = "")
        {
            var factory = _jobFactories.FirstOrDefault(f => f.JobType.Equals(type, StringComparison.OrdinalIgnoreCase));
            return factory == null ? throw new NoMatchingJobFactoryException(type) : factory.CreateJob(type, id, name, description);
        }

        public Job CreateJob<TInput>(string type, string id, TInput input, string name = "", string description = "")
        {
            var factory = _jobWithInputFactories.FirstOrDefault(f => f.JobType.Equals(type, StringComparison.OrdinalIgnoreCase));
            return factory == null ? throw new NoMatchingJobFactoryException(type) : factory.CreateJob(type, id, input, name, description);
        }
    }
}
