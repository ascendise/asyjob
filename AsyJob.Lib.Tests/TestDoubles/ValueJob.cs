using AsyJob.Lib.Jobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyJob.Lib.Tests.TestDoubles
{
    internal class ValueJob<T>(string id, string name, T value, string description = "") : Job(id, name, description)
    {
        public T Value { get; private set; } = value;

        public ValueJob(string id, T value, string description = "") : this(id, id, value, description) { }

        public override void Update(Job job)
        {
            base.Update(job);
            Value = (job as ValueJob<T>)!.Value;
        }
    }
}
