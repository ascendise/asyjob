using System.Dynamic;

namespace AsyJob.Lib.Client.Abstract.Jobs
{
    public class JobResponse(string id, string name, string description, string progressStatus, 
        IDictionary<string, object?>? input, IDictionary<string, object?>? output, Exception? error)
    {
        public string Id { get; private set; } = id;
        public string Name { get; private set; } = name;
        public string Description { get; private set; } = description;
        public string ProgressStatus { get; private set; } = progressStatus;
        public IDictionary<string, object?>? Input { get; private set; } = input;
        public IDictionary<string, object?>? Output { get; private set; } = output;
        public Exception? Error { get; private set; } = error;
    }
}
