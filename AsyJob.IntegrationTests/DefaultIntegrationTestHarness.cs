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
    internal class DefaultIntegrationTestHarness
    {
        protected IConfiguration Configuration { get; private set; }

        public DefaultIntegrationTestHarness()
        {
            Configuration = GetConfiguration();
        }

        private static IConfiguration GetConfiguration() => new ConfigurationBuilder()
                .AddJsonFile("appsettings.Tests.json")
                .Build();

        [SetUp]
        public virtual Task SetUp() 
        {
            ClearDatabase();
            return Task.CompletedTask;
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
        public virtual Task TearDown() 
        {
            ClearDatabase();
            return Task.CompletedTask;
        }
    }
}
