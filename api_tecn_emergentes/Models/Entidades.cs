using System.Collections.Generic;
using System.Net;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace api_tecn_emergentes.Models
{
    public class Entidades
    {
        [BsonElement("_id")]
        public ObjectId id { get; set; }
        [BsonElement("id_entidad")]
        public int id_entidad { get; set; }
        [BsonElement("nombre")]
        public string nombre { get; set; }
        [BsonElement("reactores")]
        public List<Reactor> reactores { get; set; }
        [BsonElement("sensores")]
        public Sensor sensores { get; set; }
    }

    public class Reactor
    {
        public IPAddress ip_reactor { get; set; }
        public string tipo { get; set; }
        public bool estado { get; set; }
    }
    public class Sensor
    {
        public IPAddress ip_sensor { get; set; }
        public Temperature temp { get; set; }
        public Humidity hum { get; set; }
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
