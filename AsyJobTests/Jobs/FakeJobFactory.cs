using AsyJob.Jobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyJobTests.Jobs
{
    internal class FakeJobFactory(string factoryName, string jobType) : IJobFactory
    {
        public string FactoryName { get; } = factoryName;
        public string JobType { get; } = jobType; 

        public Job CreateJob(string type, string id, string name = "", string description = "")
        {
            return new FakeJobFactoryOutputJob(id, name, type, FactoryName, description);
        }

        public Job CreateJob<TInput>(string type, string id, TInput input, string name = "", string description = "")
        {
           return new FakeJobFactoryOutputExtendedJob<TInput>(id, name, input, type, FactoryName, description); 
        }
    }

    internal class FakeJobFactoryOutputJob(string id, string name, string jobType, string factoryName, string description = "") 
        : Job(id, name, description)
    {
        public string JobType { get; set; } = jobType;
        public string FactoryName { get; set; } = factoryName;
    }

    internal class FakeJobFactoryOutputExtendedJob<TInput>(string id, string name, TInput input, string jobType, string factoryName, string description = "")
        : FakeJobFactoryOutputJob(id, name, jobType, factoryName, description), IInput<TInput>
    {
        public TInput Input { get; } = input;
    }

}
