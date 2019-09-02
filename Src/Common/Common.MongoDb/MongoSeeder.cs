using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.MongoDb
{
    public class MongoSeeder : IDatabaseSeeder
    {
        protected readonly IMongoDatabase Database;

        public MongoSeeder(IMongoDatabase database)
        {
            Database = database;
        }

        public async Task SeedAsync()
        {
            var collectionsCursor = await Database.ListCollectionsAsync();
            var collections = await collectionsCursor.ToListAsync();
            if (collections.Any())
            {
                return;
            }
            await CustomSeedAsync();
        }

        protected virtual async Task CustomSeedAsync()
        {
            await Task.CompletedTask;
        }
    }
}
