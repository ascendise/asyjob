namespace AsyJob.Web.HAL
{
    public abstract class HALDocument
    {
        public Links Links { get; set; } = [];
        public Embedded Embedded { get; set; } = [];
    }
}
