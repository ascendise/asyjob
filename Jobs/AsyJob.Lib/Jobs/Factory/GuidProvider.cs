namespace AsyJob.Lib.Jobs.Factory
{
    /// <summary>
    /// Implementation of IGuidProvider using builting <see cref="Guid"/>
    /// </summary>
    public class GuidProvider : IGuidProvider
    {
        public Guid GetGuid()
            => Guid.NewGuid();
    }
}
