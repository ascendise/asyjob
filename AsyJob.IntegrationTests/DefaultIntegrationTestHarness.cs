using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyJob.IntegrationTests
{
    /// <summary>
    /// Test harness that handles running all SetUp/TearDown tasks
    /// </summary>
    /// <remarks>The test assumes that there is ONE test run in the class</remarks>
    internal class DefaultIntegrationTestHarness
    {
        protected IConfiguration Configuration { get; private set; } = GetConfiguration();

        private readonly List<ISetUp> _setUps = [];
        private readonly List<ITearDown> _tearDowns = [];

        public DefaultIntegrationTestHarness(IEnumerable<ISetUp>? setUps = null, IEnumerable<ITearDown>? tearDowns = null)
        {
            ConfigureSetUp(setUps);
            ConfigureTearDown(tearDowns);
        }

        private void ConfigureSetUp(IEnumerable<ISetUp>? setUps)
        {
            _setUps.Add(new ClearMongoDbSetUpTearDown(Configuration));
            if (setUps is not null)
                _setUps.AddRange(setUps);
        }

        private void ConfigureTearDown(IEnumerable<ITearDown>? tearDowns)
        {
            _tearDowns.Add(new ClearMongoDbSetUpTearDown(Configuration));
            if (tearDowns is not null)
                _tearDowns.AddRange(tearDowns);
        }

        private static IConfiguration GetConfiguration() 
            => new ConfigurationBuilder()
                .AddJsonFile("appsettings.Tests.json")
                .Build();

        [SetUp]
        public async virtual Task SetUp() 
        {
            foreach(var setUp in _setUps)
            {
                await setUp.SetUp();
            }
        }

        [TearDown]
        public async virtual Task TearDown() 
        {
            foreach(var tearDown in _tearDowns)
            {
                await tearDown.TearDown();
            }
        }
    }
}
