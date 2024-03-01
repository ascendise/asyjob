namespace AsyJob.Jobs
{
    /// <summary>
    /// Thrown when the input provided by the client is not valid for the requested job type
    /// Example: A user creates a job that requires an integer as input, but the client passed a DateTime
    /// </summary>
    /// <param name="expected"></param>
    /// <param name="actual"></param>
    public class JobInputMismatchException(Type expected, Type actual)
        : Exception($"Expected type {expected.Name}, but was {actual.Name}")
    {
    }
}
