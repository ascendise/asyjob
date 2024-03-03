using AsyJob;
using AsyJob.Jobs;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyJobTests.Jobs.Test_Doubles
{
    internal class FakeJobFactory(string factoryName, string jobType) : IJobFactory
    {
        public string FactoryName { get; } = factoryName;
        public string JobType { get; } = jobType; 

        public Job CreateJob(string type, string id, string name = "", string description = "")
        {
            return new FakeJobFactoryOutputJob(id, name, type, FactoryName, description);
        }

    }

    internal class FakeJobWithInputFactory(string factoryName, string jobType) : IJobWithInputFactory
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

    internal class FakeJobFactoryOutputJob(string id, string name, string jobType, string factoryName, string description = "") 
        : Job(id, name, description)
    {
        public string JobType { get; set; } = jobType;
        public string FactoryName { get; set; } = factoryName;
    }

    internal class FakeJobFactoryOutputExtendedJob(string id, string name, FakeFactoryJobInput input, string jobType, string factoryName, string description = "")
        : FakeJobFactoryOutputJob(id, name, jobType, factoryName, description), IInput<FakeFactoryJobInput>
    {
        public FakeFactoryJobInput Input { get; } = input;
    }

}
