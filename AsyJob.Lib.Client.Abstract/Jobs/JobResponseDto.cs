using System.Dynamic;

namespace AsyJob.Lib.Client.Abstract.Jobs
{
    public class JobResponseDto(string id, string name, string description, string progressStatus, 
        ExpandoObject? input, ExpandoObject? output, Exception? error)
    {
        public string Id { get; private set; } = id;
        public string Name { get; private set; } = name;
        public string Description { get; private set; } = description;
        public string ProgressStatus { get; private set; } = progressStatus;
        public ExpandoObject? Input { get; private set; } = input;
        public ExpandoObject? Output { get; private set; } = output;
        public Exception? Error { get; private set; } = error;
    }
}
