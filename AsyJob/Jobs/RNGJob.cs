using AsyJob.Lib.Jobs;
using AsyJob.Lib.Jobs.Factory;

namespace AsyJob.Jobs
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

        public Job CreateJobWithInput(string type, string id, dynamic input, string name = "", string description = "")
        {
            int periodMillis = DynamicExtensions.TryGetValue<int>(input, nameof(RNGInput.PeriodMillis))
                ?? throw new JobInputMismatchException(nameof(periodMillis), typeof(int));
            int iterations = DynamicExtensions.TryGetValue<int>(input, nameof(RNGInput.Iterations))
                ?? throw new JobInputMismatchException(nameof(iterations), typeof(int));
            long min = DynamicExtensions.TryGetValue<long>(input, nameof(RNGInput.Min))
                ?? throw new JobInputMismatchException(nameof(min), typeof(long));
            long max = DynamicExtensions.TryGetValue<long>(input, nameof(RNGInput.Max))
                ?? throw new JobInputMismatchException(nameof(max), typeof(long));
            var rngInput = new RNGInput(periodMillis, iterations, min, max);
            return new RNGJob(rngInput, id, name, description);
        }
    }
}
