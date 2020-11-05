using dotnetcondapackage.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LegacyDatasystemDotNetMongoB.Helpers
{
    public class MongoDatabaseHelper
    {
        string connection;
        string database;
        private MongoDatabaseHelper(IDatabaseSettings settings)
        {
             connection = settings.ConnectionString;
             database = settings.DatabaseName;
        }
        private IMongoDatabase GetDatabase()
        {
            MongoClient mongoClient = new MongoClient(connection);
            return mongoClient.GetDatabase(database);
        }

        public IMongoCollection<BsonDocument> GetCollection(string collection)
        {
            return GetDatabase().GetCollection<BsonDocument>(collection);
        }

        public IMongoCollection<TDocument> GetCollection<TDocument>(string collection)
        {
            return GetDatabase().GetCollection<TDocument>(collection);
        }
    }
}
