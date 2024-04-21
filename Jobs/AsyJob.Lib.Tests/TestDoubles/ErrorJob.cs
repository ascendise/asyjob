using AsyJob.Lib.Jobs;

namespace AsyJob.Lib.Tests.TestDoubles
{
    public class ErrorJob : Job
    {
        public ErrorJob(string id, string description = "") : base(id, description)
        {
        }

        public ErrorJob(string id, string name, string description = "") : base(id, name, description)
        {
        }

        protected override void OnRun()
        {
            throw new TestException("This job always throws an exception");
        }
    }
}
