using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel_Management.MongoDatabase
{
    public sealed class MongoHandler
    {
        const string connectionString = "mongodb+srv://vitalis:arthur010203@cluster0.4opqwlz.mongodb.net/?retryWrites=true&w=majority";
        private static MongoHandler instance;
        static MongoClient MongoClient;
        static IMongoDatabase _database;

        MongoHandler() {
            var settings = MongoClientSettings.FromConnectionString(connectionString);
            settings.ServerApi = new ServerApi(ServerApiVersion.V1);
            MongoClient = new MongoClient(settings);
            _database = MongoClient.GetDatabase("HotelManagement");
        }

        public static MongoHandler GetInstance()
        {
            if (instance == null)
            {
                instance = new MongoHandler();
                return instance;
            }
            else
            {
                return instance;
            }
        }

        public IMongoDatabase GetDatabase(string databaseName)
        {
            return _database;
        }
        public IMongoCollection<BsonDocument> GetCollection (string collectionName)
        {
            return _database.GetCollection<BsonDocument>(collectionName);
        }
    }
}
