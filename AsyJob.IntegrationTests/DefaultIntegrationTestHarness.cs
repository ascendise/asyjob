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
    /// Test harness that handles general cleanup between tests.
    /// For example: Removes all collections created between test runs
    /// </summary>
    internal class DefaultIntegrationTestHarness(IEnumerable<ISetUp>? setUps = null, IEnumerable<ITearDown>? tearDowns = null)
    {
        protected IConfiguration Configuration { get; private set; } = GetConfiguration();

        private readonly IEnumerable<ISetUp> _setUps = setUps ?? [];
        private readonly IEnumerable<ITearDown> _tearDowns = tearDowns ?? [];

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
            ClearDatabase();
        }

        private void ClearDatabase()
        {
            var database = ConnectToDatabase();
            var collectionsCursor = database.ListCollectionNames();
            while(collectionsCursor.MoveNext())
            {
                foreach (var  collection in collectionsCursor.Current)
                    database.DropCollection(collection);
            }
        }

        private IMongoDatabase ConnectToDatabase()
        {
            var connectionString = Configuration.GetConnectionString("MongoDB");
            var client = new MongoClient(connectionString);
            var database = Configuration["DatabaseName"];
            return client.GetDatabase(database);
        }

        [TearDown]
        public async virtual Task TearDown() 
        {
            foreach(var tearDown in _tearDowns)
            {
                await tearDown.TearDown();
            }
            ClearDatabase();
        }
    }
}
