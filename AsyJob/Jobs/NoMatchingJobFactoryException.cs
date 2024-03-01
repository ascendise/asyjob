namespace AsyJob.Jobs
{
    /// <summary>
    /// Thrown when the client tries to create a job with a type, that is not supported by any <see cref="IJobFactory"/>
    /// </summary>
    public class NoMatchingJobFactoryException(string jobType) : Exception($"No matching job factory for {jobType}")
    {
    }
}
