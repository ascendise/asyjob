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
        [SetUp]
        public void SetUp() 
        {
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

        private static IMongoDatabase ConnectToDatabase()
        {
            var config = GetConfiguration();
            var connectionString = config.GetConnectionString("MongoDB");
            var client = new MongoClient(connectionString);
            var database = config["DatabaseName"];
            return client.GetDatabase(database);
        }

        private static IConfiguration GetConfiguration() => new ConfigurationBuilder()
                .AddJsonFile("appsettings.Tests.json")
                .Build();

        [TearDown]
        public void TearDown() 
        {
            ClearDatabase();
        }
    }
}
