namespace AsyJob.Lib.Auth
{
    public readonly struct Right(string resource, Operation ops)
    {
        public string Resource { get; } = resource;
        public Operation Ops { get; } = ops;
    }

}
