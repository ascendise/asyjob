namespace AsyJob.Jobs
{
    public interface IJobFactory
    {
        public string JobType { get; }
        Job CreateJob(string type, string id, string name = "", string description = "");
        Job CreateJob<TInput>(string type, string id, TInput input, string name = "", string description = "");
    }

    public class JobFactory(IEnumerable<IJobFactory> jobFactories) : IJobFactory
    {
        public string JobType => "Job";
        private readonly IEnumerable<IJobFactory> _jobFactories = jobFactories;

        public Job CreateJob(string type, string id, string name = "", string description = "")
        {
            var factory = _jobFactories.Single(f => f.JobType.Equals(type, StringComparison.OrdinalIgnoreCase));
            return factory.CreateJob(type, id, name, description);
        }

        public Job CreateJob<TInput>(string type, string id, TInput input, string name = "", string description = "")
        {
            return null!;
        }
    }
}
