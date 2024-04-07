using System.Collections;

namespace AsyJob.Web.HAL
{
    public class Links : IEnumerable<Link>
    {
        private readonly Dictionary<string, Link> _links = [];
        private int counter = 1;

        public IEnumerator<Link> GetEnumerator()
            => _links.Values.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => _links.Values.GetEnumerator();

        public void Add(Link link)
            => _links.Add(link.Name ?? (counter++).ToString(), link);

        public void Remove(string linkName)
            => _links.Remove(linkName);

        public Link? Find(string linkName)
            => _links[linkName];
    }
}
