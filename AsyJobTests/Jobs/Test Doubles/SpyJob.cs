using AsyJob.Jobs;
using Microsoft.VisualStudio.TestPlatform.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AsyJobTests.Jobs.Test_Doubles
{
    internal class SpyJob : Job, IOutput<SpyJob.SpyResult>
    {
        public SpyResult Result { get; private set; } = new();
        private SpyJobState? _onCreation;
        private SpyJobState? _onPreRun;
        private SpyJobState? _onRun;
        private SpyJobState? _onPostRun;

        public SpyJob(string id, string description = "") : this(id, id, description)
        {
        }

        public SpyJob(string id, string name, string description = "") : base(id, name, description)
        {
            _onCreation = new SpyJobState(this.Status, this.Error);
        }

        protected override void OnPreRun()
        {
            _onPreRun = new SpyJobState(this.Status, this.Error);
        }

        protected override void OnRun()
        {
            _onRun = new SpyJobState(this.Status, this.Error);
        }

        protected override void OnPostRun()
        {
            _onPostRun = new SpyJobState(this.Status, this.Error);
            var invalidState = new TestException("The Spy Job has missing states");
            var jobRun = new SpyJobRun(
                _onCreation ?? throw invalidState,
                _onPreRun ?? throw invalidState,
                _onRun ?? throw invalidState,
                _onPostRun ?? throw invalidState
            );
            Result.AddJobRun(jobRun);
        }

        internal class SpyResult
        {
            public int RunCount { get => Runs.Count; }
            public List<SpyJobRun> Runs { get; private set; } = [];

            public void AddJobRun(SpyJobRun run)
                => Runs.Add(run);
        }

        internal readonly struct SpyJobRun(SpyJobState onCreation, SpyJobState onPreRun, SpyJobState onRun, SpyJobState onPostRun)
        {
            public SpyJobState OnCreation { get; } = onCreation;
            public SpyJobState OnPreRun { get; } = onPreRun;
            public SpyJobState OnRun { get; } = onRun;
            public SpyJobState OnPostRun { get; } = onPostRun;
        }

        internal readonly struct SpyJobState(ProgressStatus status, Exception? error)
        {
            public ProgressStatus Status { get; } = status;
            public Exception? Error { get; } = error;
        }
    }
}
