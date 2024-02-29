namespace AsyJob.Jobs
{
    public class JobInputMismatchException(Type expected, Type actual)
        : Exception($"Expected type {expected.Name}, but was {actual.Name}")
    {
    }
}
