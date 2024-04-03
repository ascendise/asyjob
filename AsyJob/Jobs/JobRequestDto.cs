using System.Dynamic;

namespace AsyJob.Web.Jobs
{
    public class JobRequestDto(string jobtype, string name = "", string description = "", ExpandoObject? input = null)
    {
        public string Name { get; private set; } = name;
        public string Description { get; private set; } = description;
        public string JobType { get; private set; } = jobtype;
        public ExpandoObject? Input { get; private set; } = input;
    }
}
