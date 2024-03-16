using System.Runtime.CompilerServices;

namespace AsyJob.Lib.Jobs
{
    public abstract class Job(string id, string name, string description = "")
    {
        public string Id { get; private set; } = id;
        public string Name { get; private set; } = name;
        public string Description { get; private set; } = description;
        public ProgressStatus Status { get; private set; } = ProgressStatus.Created;
        public bool Finished { get => Status == ProgressStatus.Done || Status == ProgressStatus.Error; }
        /// <summary>
        /// If the job failed and the ProgressStatus is set to Error, then the property Error
        /// contains the exception that was caught inside the job.
        /// </summary>
        public Exception? Error { get; private set; }

        /// <summary>
        /// Might be called by a job to signal to the Job Pool that the job has new values and the store has to be updated
        /// If this event is never raised, the job only gets persisted on creation and after execution by default.
        /// </summary>
        public event UpdateEventHandler? OnUpdate;

        public Job(string id, string description = "") : this(id, id, description) { }

        /// <summary>
        /// Intended to be run in its own thread.
        /// Is responsible for acting on the job data and updating result and status
        /// Job specific logic is implemented by overriding the On*Run methods
        /// </summary>
        public void Run()
        {
            try
            {
                Status = ProgressStatus.Waiting;
                OnPreRun();
                Status = ProgressStatus.Running;
                OnRun();
                OnPostRun();
                Status = ProgressStatus.Done;
            }
            catch (Exception ex)
            {
                Status = ProgressStatus.Error;
                Error = ex;
            }
        }

        /// <summary>
        /// Is ran during the <see cref="ProgressStatus.Waiting"/>.
        /// Intended for some preparations before the run like fetching data
        /// </summary>
        protected virtual void OnPreRun() { }

        /// <summary>
        /// Is ran during the <see cref="ProgressStatus.Running"/>.
        /// Intended for the main logic of the job like complex calculations
        /// </summary>
        protected virtual void OnRun() { }

        /// <summary>
        /// Is ran after <see cref="OnRun"/> but before the <see cref="Status"/> is set
        /// After this step, the <see cref="Status"/> will either be <see cref="ProgressStatus.Done"/>
        /// or, if an exception was thrown during execution of the job, <see cref="ProgressStatus.Error"/>
        /// </summary>
        protected virtual void OnPostRun() { }

        /// <summary>
        /// Updates the current jobs value.
        /// This method only allows <see cref="Job"/>s of the same type. So a DiceRollJob would only accept DiceRollJobs as argument
        /// </summary>
        /// <param name="job"></param>
        public virtual void Update(Job job)
        {
            if (job.GetType() != GetType())
            {
                throw new ArgumentException("Need to be the same type", nameof(job));
            }
            Id = job.Id;
            Name = job.Name;
            Description = job.Description;
            Status = job.Status;
            Error = job.Error;
        }

        protected void RaiseUpdateEvent(UpdateEventArgs e)
        {
            OnUpdate?.Invoke(this, e);
        }
    }

    public class UpdateEventArgs(Job job) : EventArgs
    {
        public Job Job { get; private set; } = job;
    }

    public delegate void UpdateEventHandler(object sender, UpdateEventArgs e);
}