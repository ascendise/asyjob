namespace AsyJob.Lib.Jobs
{
    /// <summary>
    /// Extends a job to allow output of a calculation
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IOutput<T>
    {
        /// <summary>
        /// Result of the job. If calculation of output is not finished, the output may be null
        /// </summary>
        T? Output { get; }
    }
}