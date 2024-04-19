using AsyJob.Lib;
using AsyJob.Lib.Jobs;
using AsyJob.Lib.Jobs.Factory;
using System.Security.Cryptography;

namespace AsyJob.Web.Jobs
{
    /// <summary>
    /// Job for counting up a number in a set amount of time. 
    /// Used for testing concurrent write stuff and just general for observing change during job execution.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="name"></param>
    /// <param name="input"></param>
    /// <param name="description"></param>
    public class CounterJob(string id, string name, CounterJobInput input, string description = "") : Job(id, name, description), IInput<CounterJobInput>, IOutput<CounterJobOutput>
    {
        public CounterJob(string id, string name, CounterJobInput input) : this(id, name, input, "") { }

        public CounterJobInput Input { get; private set; } = input;

        public CounterJobOutput? Output { get; set; }

        protected override void OnPreRun()
        {
            Output = new(0);
        }

        protected override void OnRun()
        {
            while (Output!.Progress < Input.Goal)
            {
                Thread.Sleep(Input.DelayMs);
                Output.Progress += Input.Increment;
                RaiseUpdateEvent(new(this));
            }
        }

        public override void Update(Job job)
        {
            base.Update(job);
            var counterJob = (job as CounterJob)!;
            Input = counterJob.Input;
            Output = counterJob.Output;
        }

        public IDictionary<string, object?> GetInputDict()
            => new Dictionary<string, object?>()
            {
                { nameof(Input.Increment), Input.Increment },
                { nameof(Input.Value), Input.Value },
                { nameof(Input.Goal), Input.Goal },
                { nameof(Input.DelayMs), Input.DelayMs },
            };

        IDictionary<string, object?> IOutput.GetOutputDict()
            => new Dictionary<string, object?>()
            {
                { nameof(Output.Progress), Output?.Progress }
            };
    }

    public class CounterJobInput(int delayms = 0, int startValue = 0, int goal = int.MaxValue, int increment = 1)
    {
        public int DelayMs { get; private set; } = delayms;
        public int Value { get; private set; } = startValue;
        public int Goal { get; private set; } = goal;
        public int Increment { get; private set; } = increment;
    }

    public class CounterJobOutput(int progress)
    {
        public int Progress { get; set; } = progress;
    }

    public class CounterJobFactory : IJobWithInputFactory
    {
        public string JobType => nameof(CounterJob);

        public Job CreateJobWithInput(string _, string id, IDictionary<string, object?> input, string name = "", string description = "")
        {
            int delay = input.Get<int?>(nameof(CounterJobInput.DelayMs))
                ?? 0;
            int value = input.Get<int?>(nameof(CounterJobInput.Value)) 
                ?? 0;
            int goal = input.Get<int?>(nameof(CounterJobInput.Goal)) as int?
                ?? int.MaxValue;
            int increment = input.Get<int?>(nameof(CounterJobInput.Increment)) 
                ?? 1;
            var jobInput = new CounterJobInput(delay, value, goal, increment);
            return new CounterJob(id, name, jobInput, description);
        }
    }
}
