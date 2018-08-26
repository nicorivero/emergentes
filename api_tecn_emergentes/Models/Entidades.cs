using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace api_tecn_emergentes.Models
{
    public class Entidades
    {
        public ObjectId id { get; set; }
        [BsonElement("_id")]
        public int id_entidad { get; set; }
        [BsonElement("id_entidad")]
        public string nombre { get; set; }
        [BsonElement("nombre")]
        public List<Reactor> reactores { get; set; }
        [BsonElement("reactores")]
        public Temperature temp { get; set; }
        [BsonElement("temp")]
        public Humidity hum { get; set; }
        [BsonElement("hum")]
    }

    public class Reactor
    {
        public string type { get; set; }
        public bool state { get; set; }
    }
    public class Temperature
    {
        public double min { get; set; }
        public double max { get; set; }
    }
    public class Humidity
    {
        public double min { get; set; }
        public double max { get; set; }
    }
}
