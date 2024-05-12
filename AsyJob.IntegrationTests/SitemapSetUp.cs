using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyJob.IntegrationTests
{
    /// <summary>
    /// SetUp routine that fetches the Sitemap used to get the links for the test
    /// </summary>
    internal class SitemapSetUp(HttpClient sut) : ISetUp
    {
        public SitemapResponse Sitemap { get; private set; } = null!;
        private readonly HttpClient _sut = sut;

        public async Task SetUp()
        {
            var sitemapResponse = await _sut.GetAsync("api");
            Assert.That(sitemapResponse.IsSuccessStatusCode, Is.True, "Failed to fetch Sitemap");
            var sitemap = JsonConvert.DeserializeObject<SitemapResponse>(await sitemapResponse.Content.ReadAsStringAsync());
            Assert.That(sitemap, Is.Not.Null, "Sitemap response is empty");
            Sitemap = sitemap!;
        }
    }
}
