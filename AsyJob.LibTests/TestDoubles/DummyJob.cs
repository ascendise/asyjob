using AsyJob.Lib.Jobs;

namespace AsyJob.Lib.Tests.TestDoubles
{
    public class DummyJob : Job
    {
        public DummyJob(string id, string description = "") : base(id, description)
        {
        }

        public DummyJob(string id, string name, string description = "") : base(id, name, description)
        {
        }
    }
}
