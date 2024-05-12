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
        protected HttpClient Sut { get => _systemUnderTestSetUp.Sut; }
        protected List<ISetUp> SetUps { get; private set; } = [];
        protected List<ITearDown> TearDowns { get; private set; } = [];

        private readonly ClearMongoDbSetUpTearDown _clearDbSetUp = new(GetConfiguration());
        private readonly SystemUnderTestConstructionSetUp _systemUnderTestSetUp = new();

        public DefaultIntegrationTestHarness(IEnumerable<ISetUp>? setUps = null, IEnumerable<ITearDown>? tearDowns = null)
        {
            ConfigureSetUp(setUps);
            ConfigureTearDown(tearDowns);
            PreSetUpConfiguration();
        }

        private void ConfigureSetUp(IEnumerable<ISetUp>? setUps)
        {
            if (setUps is not null)
                SetUps.AddRange(setUps);
        }

        private void ConfigureTearDown(IEnumerable<ITearDown>? tearDowns)
        {
            TearDowns.Add(_clearDbSetUp);
            if (tearDowns is not null)
                TearDowns.AddRange(tearDowns);
        }

        private void PreSetUpConfiguration()
        {
            _clearDbSetUp.SetUp();
            _systemUnderTestSetUp.SetUp();
        }

        private static IConfiguration GetConfiguration() 
            => new ConfigurationBuilder()
                .AddJsonFile("appsettings.Tests.json")
                .Build();

        [SetUp]
        public async virtual Task SetUp() 
        {
            foreach(var setUp in SetUps)
            {
                await setUp.SetUp();
            }
        }

        [TearDown]
        public async virtual Task TearDown() 
        {
            foreach(var tearDown in TearDowns)
            {
                await tearDown.TearDown();
            }
        }
    }
}
