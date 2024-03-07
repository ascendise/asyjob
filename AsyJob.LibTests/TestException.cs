namespace AsyJob.Tests
{
    /// <summary>
    /// Used in test double classes to avoid mix up with exceptions thrown from the system under test
    /// </summary>
    internal class TestException : Exception
    {
        public TestException() { }
        public TestException(string message) : base(message) { }
    }
}
