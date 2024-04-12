using System.Collections;

namespace AsyJob.Web.HAL
{
    public class Embedded : IEnumerable<Embed>
    {
        private readonly Dictionary<string, Embed> _embedded = [];

        public IEnumerator<Embed> GetEnumerator()
            => _embedded.Values.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => _embedded.Values.GetEnumerator();

        public void Add(Embed embed)
            => _embedded.Add(embed.ResourceName, embed);

        public void Remove(string embedName)
            => _embedded.Remove(embedName);

        public Embed? Find(string embedName)
            => _embedded[embedName];
    }
}
