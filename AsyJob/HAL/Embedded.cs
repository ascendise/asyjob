using System.Collections;

namespace AsyJob.Web.HAL
{
    public class Embedded : IEnumerable<Embed>
    {
        private readonly Dictionary<string, Embed> _embedded = [];
        private int counter = 0;

        public IEnumerator<Embed> GetEnumerator()
            => _embedded.Values.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => _embedded.Values.GetEnumerator();

        public void Add(Embed embed)
            => _embedded.Add(embed.Name ?? (counter++).ToString(), embed);

        public void Remove(string embedName)
            => _embedded.Remove(embedName);

        public Embed? Find(string embedName)
            => _embedded[embedName];
    }
}
