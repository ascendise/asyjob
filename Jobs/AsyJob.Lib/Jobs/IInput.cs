namespace AsyJob.Lib.Jobs
{
    /// <summary>
    /// Extends a job to allow input data to influence the calculation.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IInput<T> : IInput where T : class
    {
        T Input { get; }
    }

    public interface IInput
    {
        public IDictionary<string, object?> GetInputDict();
    }
}