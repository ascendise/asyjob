using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyJob.IntegrationTests
{
    internal class ClearMongoDbSetUpTearDown(IConfiguration configuration) : ISetUp, ITearDown
    {
        private readonly IConfiguration _configuration = configuration;

        public Task SetUp()
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
            var connectionString = _configuration.GetConnectionString("MongoDB");
            var client = new MongoClient(connectionString);
            var database = _configuration["DatabaseName"];
            return client.GetDatabase(database);
        }

        public Task TearDown()
        {
            ClearDatabase();
            return Task.CompletedTask;
        }
    }
}
