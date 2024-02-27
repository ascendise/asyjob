using System.Runtime.InteropServices;

namespace AsyJob.Jobs
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
            catch(Exception ex)
            {
                this.Status = ProgressStatus.Error;
                this.Error = ex;
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
    }

    public enum ProgressStatus
    {
        Created, Waiting, Running, Done, Error
    }

    /// <summary>
    /// Extends a job to allow input data to influence the calculation.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IInput<T>
    {
        T Input { get; }
    }

    /// <summary>
    /// Extends a job to allow output of a calculation
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IOutput<T>
    {
        /// <summary>
        /// Result of the job. If calculation of output is not finished, the output may be null
        /// </summary>
        T? Output { get; }
    }
} 