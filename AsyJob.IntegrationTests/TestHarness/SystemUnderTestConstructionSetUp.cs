using Amazon.Runtime.Internal.Transform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyJob.IntegrationTests.TestHarness
{
    internal class SystemUnderTestConstructionSetUp : ISetUp
    {
        public HttpClient Sut { get; private set; } = null!;

        public Task SetUp()
        {
            Sut = new WebApplicationTestFactory<Program>().CreateClient();
            Assert.That(Sut, Is.Not.Null, "Failed to construct System under Test");
            return Task.CompletedTask;
        }
    }
}
