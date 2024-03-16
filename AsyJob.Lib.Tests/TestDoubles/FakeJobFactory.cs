using AsyJob.Lib.Jobs;
using AsyJob.Lib.Jobs.Factory;
using AsyJob.Lib.Runner;

namespace AsyJob.Lib.Tests.TestDoubles
{
    public class FakeJobFactory(string factoryName, string jobType) : IJobFactory
    {
        public string FactoryName { get; } = factoryName;
        public string JobType { get; } = jobType;

        public Job CreateJob(string type, string id, string name = "", string description = "")
        {
            return new FakeJobFactoryOutputJob(id, name, type, FactoryName, description);
        }

    }

    public class FakeJobWithInputFactory(string factoryName, string jobType) : IJobWithInputFactory
    {
        public string FactoryName { get; } = factoryName;
        public string JobType { get; } = jobType;


        public Job CreateJobWithInput(string type, string id, dynamic input, string name = "", string description = "")
        {
            var checknum = DynamicExtensions.TryGetValue<int?>(input, "CheckNum") ?? throw new JobInputMismatchException(nameof(input.CheckNum), typeof(int));
            var jobInput = new FakeFactoryJobInput(checknum);
            return new FakeJobFactoryOutputExtendedJob(id, name, jobInput, type, FactoryName, description);
        }
    }

    public class FakeJobFactoryOutputJob(string id, string name, string jobType, string factoryName, string description = "")
        : Job(id, name, description)
    {
        public string JobType { get; set; } = jobType;
        public string FactoryName { get; set; } = factoryName;
    }

    public class FakeJobFactoryOutputExtendedJob(string id, string name, FakeFactoryJobInput input, string jobType, string factoryName, string description = "")
        : FakeJobFactoryOutputJob(id, name, jobType, factoryName, description), IInput<FakeFactoryJobInput>
    {
        public FakeFactoryJobInput Input { get; } = input;
    }

    public class FakeFactoryJobInput(int checknum)
    {
        public int CheckNum { get; } = checknum;
    }


}
