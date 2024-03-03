namespace AsyJob.Jobs
{
    public class JobRequestDto(string jobtype, string name = "", string description = "", dynamic? input = null)     
    {
        public string Name { get; private set; } = name;
        public string Description { get; private set; } = description;
        public string JobType { get; private set; } = jobtype;
        public dynamic? Input { get; private set; } = input;
    }

    public class JobResponseDto(Job job)
    {
        public Job Job { get; private set; } = job;
    }
}
