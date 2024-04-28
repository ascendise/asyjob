namespace AsyJob.Web
{
    public interface IMapper<TSrc, TDst>
    {
        TDst Map(TSrc src);
    }
}
