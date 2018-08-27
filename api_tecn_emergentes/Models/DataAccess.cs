using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver;
namespace api_tecn_emergentes.Models
{
    public class DataAccess
    {
        MongoClient _client;
        IMongoDatabase _db;

        //Parametrizacion inicial, en el constructor se conecta a la base de datos y se inicializa la bbdd de documentos bson.
        public DataAccess()
        {
            _client = new MongoClient("mongodb://localhost:27017");
            _db = _client.GetDatabase("emergentes");
        }

        //Obtiene una coleccion de documentos (la coleccion como objeto, no la lista de documentos internos de dicha coleccion).
        public IMongoCollection<BsonDocument> GetCollection(string _name)
        {
            IMongoCollection<BsonDocument> collection = _db.GetCollection<BsonDocument>(_name);
            return collection;
        }

        //Obtiene todos los documentos de una coleccion dada como parametro en formato BSON para su tratamiento.
        public List<BsonDocument> GetAllDocuments(string _collection_name)
        {
            var _collection = GetCollection(_collection_name);
            var _documentsList = _collection.Find(new BsonDocument()).ToList();
            return _documentsList;
        }

        //Obtiene un documento particular pasandole como parametro el campo a filtrar, el valor y el nombre de la coleccion.
        public BsonDocument GetDocument(string _field, int _value, string _collection_name)
        {
            var filter = Builders<BsonDocument>.Filter.Eq(_field, _value);
            var doc = GetCollection(_collection_name).Find(new BsonDocument()).ToList();
            var document = GetCollection(_collection_name).Find(filter).First();
            return document;
        }

        //Metodo para insertar una nueva entidad, no se insertan nuevos "parametros", solo entidades completas.
        public string InsertDocument(string _collection_name, BsonDocument _document)
        {
            try
            {
                GetCollection(_collection_name).InsertOne(_document);
                return "Insercion Correcta";
            }
            catch(Exception ex)
            {
                return "Error al insertar. Detalle:" + ex.Message;
            }
        }

        //Metodo para actualizar valores de una coleccion, indicando la coleccion a utilizar, el filtro y los datos a actualizar.
        public void UpdateDocument<T,C>(IMongoCollection<BsonDocument> _collection, string _field, C _value, string _setField, T _setValue)
        {
            var filter = Builders<BsonDocument>.Filter.Eq(_field, _value);
            var update = Builders<BsonDocument>.Update.Set(_setField,_setValue);
            _collection.UpdateOne(filter, update);
        }

        //Metodo que borra una entidad completa, no se borraran partes del documento segun alcance.
        public string DeleteDocument<T>(string _collection_name, string _field, T _value)
        {
            try
            {
                GetCollection(_collection_name).DeleteOne(Builders<BsonDocument>.Filter.Eq(_field, _value));
                return "Eliminacion Exitosa";
            }
            catch(Exception ex)
            {
                return "Ha ocurrido un error al eliminar. Detalle:" + ex.Message;
            }
        }
    }
}
