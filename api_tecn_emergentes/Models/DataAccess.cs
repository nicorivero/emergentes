using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Core;
using MongoDB.Driver.Linq;
namespace api_tecn_emergentes.Models
{
    public class DataAccess
    {
        MongoClient _client;
        IMongoDatabase _db;
        
        public DataAccess()
        {
            _client = new MongoClient("mongodb://localhost:27017");
            _db = _client.GetDatabase("Emergentes");
        }

        public IMongoCollection<BsonDocument> GetCollection(string name)
        {
            var _collection = _db.GetCollection<BsonDocument>(name);
            return _collection;
        }
        public IEnumerable<Entidades> GetEntity(int id)
        {
            
            //return _db.GetCollection<Entidades>("Entidades").Find
        }

        public void UpdateEntity(Entidades e)
        {

        }

        public void CreateEntity(Entidades e)
        {

        }
    }
}
