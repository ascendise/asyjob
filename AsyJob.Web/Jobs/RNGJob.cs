using AsyJob.Lib;
using AsyJob.Lib.Jobs;
using AsyJob.Lib.Jobs.Factory;

namespace AsyJob.Web.Jobs
{
    public class RNGJob(RNGInput input, string id, string name, string description = "") : Job(id, name, description), IInput<RNGInput>, IOutput<RNGOutput>
    {
        public RNGInput Input { get; private set; } = input;
        public RNGOutput? Output { get; private set; }

        public RNGJob(RNGInput input, string id, string description = "") : this(input, id, id, description)
        {
        }

        protected override void OnRun()
        {
            var random = new Random();
            for (int i = 0; i < Input.Iterations; i++)
            {
                var num = random.NextInt64(Input.Min, Input.Max);
                Output = new RNGOutput(num);
                RaiseUpdateEvent(new(this));
                Thread.Sleep(Input.PeriodMillis);
            }
        }

        public IDictionary<string, object?> GetInputDict()
            => new Dictionary<string, object?>()
            {
                { nameof(Input.Iterations), Input.Iterations },
                { nameof(Input.Min), Input.Min },
                { nameof(Input.Max), Input.Max },
                { nameof(Input.PeriodMillis), Input.PeriodMillis }
            };

        public IDictionary<string, object?> GetOutputDict()
            => new Dictionary<string, object?>()
            {
                { nameof(Output.Result), Output?.Result }
            };
    }

    public class RNGInput(int periodMillis = 100, int iterations = 1000, long min = long.MinValue, long max = long.MaxValue)
    {
        public int PeriodMillis { get; } = periodMillis;
        public int Iterations { get; } = iterations;
        public long Min { get; } = min;
        public long Max { get; } = max;
    }

    public class RNGOutput(long result)
    {
        public long Result { get; } = result;
    }

    public class RNGJobFactory : IJobWithInputFactory
    {
        public string JobType { get; } = nameof(RNGJob);

        public Job CreateJobWithInput(string _, string id, IDictionary<string, object?> input, string name = "", string description = "")
        {
            int periodMillis = input.Get<int?>(nameof(RNGInput.PeriodMillis))
                ?? throw new JobInputMismatchException(nameof(periodMillis), typeof(int));
            int iterations = input.Get<int?>(nameof(RNGInput.Iterations)) 
                ?? throw new JobInputMismatchException(nameof(iterations), typeof(int));
            long min = input.Get<int?>(nameof(RNGInput.Min))
                ?? throw new JobInputMismatchException(nameof(min), typeof(long));
            long max = input.Get<int?>(nameof(RNGInput.Max)) 
                ?? throw new JobInputMismatchException(nameof(max), typeof(long));
            var rngInput = new RNGInput(periodMillis, iterations, min, max);
            return new RNGJob(rngInput, id, name, description);
        }
    }
}
