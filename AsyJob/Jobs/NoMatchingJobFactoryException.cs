namespace AsyJob.Jobs
{
    public class NoMatchingJobFactoryException : Exception
    {
        public NoMatchingJobFactoryException(string jobType) : base($"No matching job factory for {jobType}") { }
    }
}
