using System.Collections;

namespace AsyJob.Web.HAL
{
    public class Links : IEnumerable<KeyValuePair<string, Link>>
    {
        private readonly Dictionary<string, Link> _links = [];

        public Link? this[string index]
        {
            get => Find(index);
            set => Add(index, value ?? throw new ArgumentException("Link cannot ben null"));
        }

        public IEnumerator<KeyValuePair<string, Link>> GetEnumerator()
            => _links.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => _links.GetEnumerator();

        public void Add(string resource, Link link)
            => _links.Add(resource, link);

        public void Remove(string linkName)
            => _links.Remove(linkName);

        public Link? Find(string linkName)
            => _links[linkName];
    }
}
