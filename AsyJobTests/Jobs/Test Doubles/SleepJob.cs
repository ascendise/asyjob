using AsyJob.Jobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyJobTests.Jobs.Test_Doubles
{
    internal class SleepJob(string id, string name, SleepInput data, string description = "") : Job(id, name, description), IInput<SleepInput>
    {
        public SleepInput Input { get; private set; } = data;

        public SleepJob(string id, SleepInput data, string description = "") : this(id, id, data, description)
        {
        }

        protected override void OnRun()
        {
            Thread.Sleep(Input.Time);
        }
    }

    internal class SleepInput(int time)
    {
        /// <summary>
        /// Time in milliseconds the task should <see cref="Thread.Sleep(int)"/>
        /// </summary>
        public int Time { get; } = time;
    }
}
