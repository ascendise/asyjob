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
}
