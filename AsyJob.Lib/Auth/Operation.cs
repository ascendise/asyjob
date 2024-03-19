namespace AsyJob.Lib.Auth
{
    [Flags]
    public enum Operation
    {
        Read = 1,
        Write = 2,
        Execute = 4
    }
}
