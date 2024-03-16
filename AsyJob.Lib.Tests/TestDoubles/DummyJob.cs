using AsyJob.Lib.Jobs;

namespace AsyJob.Lib.Tests.TestDoubles
{
    public class DummyJob : Job
    {
        private DummyJob(): base("", "") { }

        public DummyJob(string id, string description = "") : base(id, description)
        {
        }

        public DummyJob(string id, string name, string description = "") : base(id, name, description)
        {
        }
    }
}
