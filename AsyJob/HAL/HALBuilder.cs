namespace AsyJob.Web.HAL
{
    public class HALBuilder
    {
        private readonly List<Link> _links = [];

        public Link.LinkBuilder NewLink(string href, bool? templated)
            => new(this, href, templated);

        public HALBuilder AddLink(Link link)
        {
            _links.Add(link);
            return this;
        }
    }
}
