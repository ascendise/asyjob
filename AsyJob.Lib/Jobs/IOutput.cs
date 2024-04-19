namespace AsyJob.Lib.Jobs
{
    /// <summary>
    /// Extends a job to allow output of a calculation
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IOutput<T> : IOutput where T : class
    {
        /// <summary>
        /// Result of the job. If calculation of output is not finished, the output may be null
        /// </summary>
        T? Output { get; }
    }

    public interface IOutput
    {
        IDictionary<string, object?> GetOutputDict();
    }
}