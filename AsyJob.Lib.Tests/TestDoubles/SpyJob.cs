using AsyJob.Lib.Jobs;

namespace AsyJob.Lib.Tests.TestDoubles
{
    public class SpyJob : Job, IOutput<SpyJob.SpyResult>
    {
        public SpyResult Output { get; private set; } = new();
        private readonly SpyJobState? _onCreation;
        private SpyJobState? _onPreRun;
        private SpyJobState? _onRun;
        private SpyJobState? _onPostRun;

        public SpyJob(string id, string description = "") : this(id, id, description)
        {
        }

        public SpyJob(string id, string name, string description = "") : base(id, name, description)
        {
            _onCreation = new SpyJobState(Status, Error);
        }

        protected override void OnPreRun()
        {
            _onPreRun = new SpyJobState(Status, Error);
        }

        protected override void OnRun()
        {
            _onRun = new SpyJobState(Status, Error);
        }

        protected override void OnPostRun()
        {
            _onPostRun = new SpyJobState(Status, Error);
            var invalidState = new TestException("The Spy Job has missing states");
            var jobRun = new SpyJobRun(
                _onCreation ?? throw invalidState,
                _onPreRun ?? throw invalidState,
                _onRun ?? throw invalidState,
                _onPostRun ?? throw invalidState
            );
            Output.AddJobRun(jobRun);
        }

        public IDictionary<string, object?> GetOutputDict()
            => new Dictionary<string, object?>()
            {
                { nameof(Output.RunCount), Output.RunCount },
                { nameof(Output.Runs), Output.Runs }
            };

        public class SpyResult
        {
            public int RunCount { get => Runs.Count; }
            public List<SpyJobRun> Runs { get; private set; } = [];

            public void AddJobRun(SpyJobRun run)
                => Runs.Add(run);
        }

        public readonly struct SpyJobRun(SpyJobState onCreation, SpyJobState onPreRun, SpyJobState onRun, SpyJobState onPostRun)
        {
            public SpyJobState OnCreation { get; } = onCreation;
            public SpyJobState OnPreRun { get; } = onPreRun;
            public SpyJobState OnRun { get; } = onRun;
            public SpyJobState OnPostRun { get; } = onPostRun;
        }

        public readonly struct SpyJobState(ProgressStatus status, Exception? error)
        {
            public ProgressStatus Status { get; } = status;
            public Exception? Error { get; } = error;
        }
    }
}
