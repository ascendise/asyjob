namespace AsyJob.IntegrationTests
{
    /// <summary>
    /// Responsible for running a TearDown task before an integration test is run
    /// </summary>
    internal interface ITearDown
    {
        Task TearDown();
    }
}
