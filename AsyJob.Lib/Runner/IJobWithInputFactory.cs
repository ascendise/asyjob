using AsyJob.Lib.Jobs;

namespace AsyJob.Lib.Runner
{
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
}
