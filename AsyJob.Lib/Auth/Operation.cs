namespace AsyJob.Lib.Auth
{
    [Flags]
    public enum Operation
    {
        None = 0,
        Read = 1,
        Write = 2,
        Execute = 4
    }
}
