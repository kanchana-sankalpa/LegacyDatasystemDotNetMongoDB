﻿using dotnetcondapackage.Models;
using LegacyDatasystemDotNetMongoB.Helpers;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver.Linq;
using System.Text.RegularExpressions;
using dotnetcondapackage.Services;
using dotnetcondapackage.Entities;


//https://www.niceonecode.com/blog/64/left-join-in-mongodb-using-the-csharp-driver-and-linq
// index https://stackoverflow.com/questions/35019313/checking-if-an-index-exists-in-mongodb
// create inex https://stackoverflow.com/questions/17807577/how-to-create-indexes-in-mongodb-via-net
//wide cart https://stackoverflow.com/questions/35205837/how-to-search-word-or-string-on-all-fields-in-document-with-mongodb

namespace LegacyDatasystemDotNetMongoB.Services
{
    public class SearchService
    {


        private IMongoDatabase _connection;
        private IMongoDatabase _connection2;
        public SearchService(IDatabaseSettings settings)
        {
           
            var client = new MongoClient(settings.ConnectionString);
            _connection = client.GetDatabase(settings.DatabaseName);

            var client2 = new MongoClient("mongodb://legsysadmin:SbIvcRSwVqFZ-dqZ8IvZE@localhost:27017/?authSource=admin&readPreference=primary&appname=MongoDB%20Compass&ssl=false");
            _connection2 = client2.GetDatabase("legacy_systems");
    
            // IMongoCollection<BsonDocument> alldocs = _connection.GetCollection<BsonDocument>("AmsLights.Project_Object");
            // alldocs.Indexes.CreateOne(new CreateIndexModel<BsonDocument>(Builders<BsonDocument>.IndexKeys.Text("$**")));

        }


        public List<BsonDocument> FindSearch(string collectionName, string searchWord)
        {
            IMongoCollection<BsonDocument> alldocs = _connection.GetCollection<BsonDocument>(collectionName);
            //var filter = Builders<BsonDocument>.Filter.Eq("$text",  "{ $search: "+ searchWord + " }" );
            var filter = Builders<BsonDocument>.Filter.Text(searchWord);
            // var query = alldocs.Find(filter).ToList();
            List<BsonDocument> query;
            try
            {
               query = alldocs.Find(filter).ToList();
            }
            catch (MongoCommandException ex)
            {
                query = new List<BsonDocument>();
            }
             
            return query;
        }

        public async Task<List<BsonDocument>> FindSearchAsync( string searchWord)
        {
            string collectionName = "AmsLights.Project_Object";
            IMongoCollection<BsonDocument> alldocs = _connection.GetCollection<BsonDocument>(collectionName);
            //var filter = Builders<BsonDocument>.Filter.Eq("$text",  "{ $search: "+ searchWord + " }" );
            var filter = Builders<BsonDocument>.Filter.Text(searchWord);
            // var query = alldocs.Find(filter).ToList();
            return await alldocs.Find(filter).ToListAsync();
           // return query;
        }

        public async Task<List<BsonDocument>> FindSearchCollectionAsync(string collectionName,string searchWord)
        {
            IMongoCollection<BsonDocument> alldocs = _connection.GetCollection<BsonDocument>(collectionName);
            var filter = Builders<BsonDocument>.Filter.Text(searchWord);
           
            List<BsonDocument> query;
           // return await alldocs.Find(filter).ToListAsync();
            try
            {
                query = await alldocs.Find(filter).ToListAsync();
            }
            catch (MongoCommandException ex)
            {

                query = new List<BsonDocument>();
            }

            return query;
        }


        public List<BsonDocument> SearchText(Dataset schemaDatases, string searchWord)
        {
           
            return null;
        }


        public string  createTextIndex(string collectionName)
        {
            string status;
            try
            {
                IMongoCollection<BsonDocument> alldocs = _connection2.GetCollection<BsonDocument>(collectionName);
                alldocs.Indexes.CreateOne(new CreateIndexModel<BsonDocument>(Builders<BsonDocument>.IndexKeys.Text("$**")));
                status = collectionName;
            }
            catch (Exception ex)
            {
               
                status = ex.ToString();
            }
            return status;
        }

        //public async Task<List<BsonDocument>> createTextIndexAsync(string collectionName)
        //{
        //    string status;
        //    try
        //    {
        //        IMongoCollection<BsonDocument> alldocs = _connection2.GetCollection<BsonDocument>(collectionName);
        //        alldocs.Indexes.CreateOne(new CreateIndexModel<BsonDocument>(Builders<BsonDocument>.IndexKeys.Text("$**")));
        //        status = collectionName;
        //    }
        //    catch (Exception ex)
        //    {

        //        status = ex.ToString();
        //    }
        //    return status;
        //}

        /*
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



        public dynamic FindSearch(string collectionName, string searchWord)
        {
            //IMongoQuery query = Query.Text(searchWord);
            //List<T> find = getCollection<T>(collectionName).Find(query).ToList();
//IMongoCollection<BsonDocument> mongoCollection = GetCollection(collectionName);
            var builder = Builders<BsonDocument>.Filter;
            //var filter = builder.Regex("description", "(java)") | builder.Regex("description", "(coffee shop)");
            //var filter = Builders<BsonDocument>.Filter.Text(searchWord);
            // List<BsonDocument> result =  mongoCollection.Find(filter).ToList();
            //List<T> find = mongoCollection<T>(collectionName).Find(searchWord).ToList();
            //return result;
            var mongoCollection = GetCollection(collectionName);
             var filter = Builders<BsonDocument>.Filter.Text(searchWord, new TextSearchOptions { CaseSensitive = false });
             return mongoCollection.Find(filter).ToList();

        }

        [Obsolete]
        public List<BsonDocument> FindSearchSync(string collectionName, string searchWord)
        {
            var mongoCollection = GetCollection(collectionName);

            //return  mongoCollection.Aggregate()
            //.Match(Builders<BsonDocument>.Filter.Text(searchWord)).ToList();
            //list inexes
            return mongoCollection.Indexes.List().ToList(); 

            var filter = Builders<BsonDocument>.Filter.Text(searchWord, new TextSearchOptions { CaseSensitive = false });

            var results = mongoCollection.Find(filter).ToList();
        }



        public async Task<IList> GetEntityIdByOriginalEmail(string collectionName, string searchWord)
        {
            var mongoCollection = GetCollection(collectionName);
            var filter = Builders<BsonDocument>.Filter.Text(searchWord);
            //return await mongoCollection.Find().ToListAsync();
            // return await mongoCollection.Find(filter).ToListAsync();

            return await mongoCollection.Aggregate()
            .Match(Builders<BsonDocument>.Filter.Text(searchWord)).ToListAsync();
        }

        // public async Task<Entity> GetEntityIdByOriginalEmail(string originalEmail)
        //{
        //  var collection = GetCollection();
        //var filter = Builders<Entity>.Filter.Regex("x", new BsonRegularExpression(originalEmail, "i"));
        //  return await collection.Find(filter).FirstOrDefaultAsync();
        //}
         */
    }

}
