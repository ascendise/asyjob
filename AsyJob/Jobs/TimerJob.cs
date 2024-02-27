using System.Reflection;

namespace AsyJob.Jobs
{
    public class TimerJob(string id, string name, TimerInput input, string description = "") 
        : Job(id, name, description), IInput<TimerInput>, IOutput<TimerOutput>
    {
        public TimerJob(string id, TimerInput input, string description = "") : this(id, id, input, description) { }

        public TimerInput Input { get; private set; } = input;

        public TimerOutput? Output { get; private set; }

        protected override void OnPreRun()
        {
            Output = new TimerOutput(DateTime.Now);
        }

        protected override void OnRun()
        {
            Thread.Sleep(Input.Delay); 
        }

        protected override void OnPostRun()
        {
            Output!.End = DateTime.Now;
        }
    }

    public class TimerInput(int delay)
    {
        /// <summary>
        /// Represents how long the TimerJob sleeps in milliseconds
        /// </summary>
        public int Delay { get; private set; } = delay;
    }

    public class TimerOutput(DateTime start)
    {
        public DateTime Start { get; set; } = start;
        public DateTime? End { get; set; }
    }
}
