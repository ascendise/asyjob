namespace AsyJob.Lib.Auth
{
    public readonly struct Right
    {
        public string Resource { get; }
        public Operation Ops { get; }
        [Flags]
        public enum Operation
        {
            Read,
            Write,
            Execute
        }
    }

}
