namespace AsyJob.Web.HAL
{
    public interface IHAL
    {
        public ILinks Links { get; set; }
        public IEmbedded Embedded { get; set; }
    }

    public interface ILinks
    {

    }

    public interface IEmbedded
    {

    }
}
