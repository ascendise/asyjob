
namespace AsyJob.Jobs
{
    public class GuidProvider : IGuidProvider
    {
        public Guid GetGuid()
            => Guid.NewGuid(); 
    }
}
