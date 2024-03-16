using AsyJob.Lib.Jobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AsyJob.Lib.Tests.TestDoubles
{
    /// <summary>
    /// Used for testing the <see cref="Job.OnUpdate"/> event
    /// and other update-behaviour
    /// /// </summary>
    internal class UpdateJob(object value, string id, string name, string description = "") : Job(id, name, description)
    {
        private object _value = value;

        /// <summary>
        /// Anytime this value gets updated, an Update-event is raised
        /// </summary>
        public object Value
        {
            get => _value;
            set
            {
                _value = value;
                RaiseUpdateEvent(new(this));
            }
        }
        public UpdateJob(object value, string id, string description = "") : this(value, id, id, description)
        {
        }

        public UpdateJob() : this(new object(), Guid.NewGuid().ToString())
        {
        }

        protected override void OnRun()
        {
            while (true)
            {
                //Loops endlessly. Because this job is mostly intended for testing update behaviour
                //and not the behaviour of job workflows
            }
        }

        public override void Update(Job job)
        {
            base.Update(job);
            var updateJob = (job as UpdateJob)!;
            Value = updateJob.Value;
        }
    }
}
