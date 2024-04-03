using System.Runtime.InteropServices;

namespace AsyJob.Web.HAL
{
    public class HALBuilder<T>(T halObj) where T : IHAL
    {
        private readonly T _halObj = halObj;
        private readonly List<Link> _links = [];
        private readonly Dictionary<string, object> _embedded = [];

        public Link.LinkBuilder<T> NewLink(string href, bool? templated)
            => new(this, href, templated);

        public HALBuilder<T> AddLink(Link link)
        {
            _links.Add(link);
            return this;
        }

        public HALBuilder<T> AddEmbedded(string name, object value)
        {
            _embedded.Add(name, value);
            return this;
        }

        public T Build()
        {
            return _halObj;
        }


    }
}
