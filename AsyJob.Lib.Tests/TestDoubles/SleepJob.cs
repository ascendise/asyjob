using AsyJob.Lib.Jobs;

namespace AsyJob.Lib.Tests.TestDoubles
{
    public class SleepJob(string id, string name, SleepInput data, string description = "") : Job(id, name, description), IInput<SleepInput>
    {
        public SleepInput Input { get; private set; } = data;

        public SleepJob(string id, SleepInput data, string description = "") : this(id, id, data, description)
        {
        }

        protected override void OnRun()
        {
            Thread.Sleep(Input.Time);
        }

        public IDictionary<string, object?> GetInputDict()
            => new Dictionary<string, object?>
            {
                { nameof(SleepInput.Time), Input.Time },
            };
    }

    public class SleepInput(int time)
    {
        /// <summary>
        /// Time in milliseconds the task should <see cref="Thread.Sleep(int)"/>
        /// </summary>
        public int Time { get; } = time;
    }
}
