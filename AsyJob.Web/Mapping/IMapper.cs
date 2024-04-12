namespace AsyJob.Web.Mapping
{
    public interface IMapper<TSrc, TDest>
    {
        TDest Map(TSrc src);
    }
}
