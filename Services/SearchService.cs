using dotnetcondapackage.Models;
using LegacyDatasystemDotNetMongoB.Helpers;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

//https://www.niceonecode.com/blog/64/left-join-in-mongodb-using-the-csharp-driver-and-linq
// index https://stackoverflow.com/questions/35019313/checking-if-an-index-exists-in-mongodb
// create inex https://stackoverflow.com/questions/17807577/how-to-create-indexes-in-mongodb-via-net
//wide cart https://stackoverflow.com/questions/35205837/how-to-search-word-or-string-on-all-fields-in-document-with-mongodb

namespace LegacyDatasystemDotNetMongoB.Services
{
    public class SearchService
    {
        // IEnumerable<Collation>[] GetAllResults()
        //{
        //            return 
        //   }
        string connection;
        string database;
       // private readonly MongoDatabaseHelper _healper;
        public SearchService(IDatabaseSettings settings)
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



        public List<BsonDocument> FindSearch(string collectionName, string searchWord)
        {
            //IMongoQuery query = Query.Text(searchWord);
            //List<T> find = getCollection<T>(collectionName).Find(query).ToList();
            IMongoCollection<BsonDocument> mongoCollection = GetCollection(collectionName);
            var builder = Builders<BsonDocument>.Filter;
            //var filter = builder.Regex("description", "(java)") | builder.Regex("description", "(coffee shop)");
            var filter = Builders<BsonDocument>.Filter.Text(searchWord);
            List<BsonDocument> result =  mongoCollection.Find(filter).ToList();
            //List<T> find = mongoCollection<T>(collectionName).Find(searchWord).ToList();
            return result;
        }

        public async Task<IList> GetEntityIdByOriginalEmail(string collectionName, string searchWord)
        {
            var mongoCollection = GetCollection(collectionName);
            var filter = Builders<BsonDocument>.Filter.Text(searchWord);
            return await mongoCollection.Find(filter).ToListAsync();
        }

        // public async Task<Entity> GetEntityIdByOriginalEmail(string originalEmail)
        //{
        //  var collection = GetCollection();
        //var filter = Builders<Entity>.Filter.Regex("x", new BsonRegularExpression(originalEmail, "i"));
        //  return await collection.Find(filter).FirstOrDefaultAsync();
        //}
    }
}
