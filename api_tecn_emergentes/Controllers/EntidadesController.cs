using System.Collections.Generic;
using System.Linq;
using api_tecn_emergentes.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json.Linq;
using System.Net;

namespace api_tecn_emergentes.Controllers
{
    [Produces("application/json")]
    [Route("api/Entidades/[action]")]
    public class EntidadesController : Controller
    {
        private DataAccess data = new DataAccess();
        
        //Obtener toda la informacion de una entidad
        [HttpGet("id={_id_entity}")]
        public JObject Get(int _id_entity)
        {
            var document = data.GetDocsWithProjection("Entidades", new string[] { "_id"}, "id_entidad", _id_entity).First().ToJson();
            return JObject.Parse(document);
        }

        //Metodo que devuelve como resultado una lista de todas las entidades dadas de alta
        [HttpGet]
        public List<JObject> Todos()
        {
            List<BsonDocument> _bsonlist = data.GetDocsWithProjection("Entidades", new string[] { "_id" });
            List<JObject> _jsonList = new List<JObject>();
            _bsonlist.ForEach(d => _jsonList.Add(JObject.Parse(d.ToJson())));
            return _jsonList;
        }

        //Eliminar una entidad
        [HttpDelete("id={_id_entity}")]
        public string Eliminar (int _id_entity)
        {
            string _responsep = data.DeleteDocument("PreEntidades","_id", _id_entity);
            string _response = data.DeleteDocument("Entidades", "id_entidad", _id_entity);
            //Eliminar Lecturas para mantener integridad referencial de la base aunque sea documental
            //--CODIGO PARA ELIMINAR LECTURAS
            return _response + " && " + _responsep ;
        }
        
        //Cargar una nueva entidad
        [HttpPost]
        public JObject Crear([FromBody] EntidadSimple e)
        {
            //Recuperar pre-entidad correspondiente
            var _preEntElements = data.GetDocument("_id",e.id_precarga,"PreEntidades").Elements.ToList();
            
            //Crear instancias de nueva variable de entidad
            Entidades e1 = new Entidades();
            e1.reactores = new List<Reactor>();
            e1.sensores = new Sensor();
            e1.sensores.temp = new Temperature();
            e1.sensores.hum = new Humidity();

            //Carga de datos de entidad nueva
            e1.id_entidad = _preEntElements[0].Value.ToInt32();
            e1.nombre = e.nombre;
            //Reactores 
            IPAddress _ipr = IPAddress.Parse(_preEntElements[1].Value.ToString());
            e1.reactores.Add(new Reactor() { tipo = "Riego", estado = false, ip_reactor = _ipr});
            e1.reactores.Add(new Reactor() { tipo = "Climatizador", estado = false, ip_reactor = _ipr });

            //Sensores
            IPAddress _ips = IPAddress.Parse(_preEntElements[2].Value.ToString());
            e1.sensores.ip_sensor = _ips;
            e1.sensores.temp.max = e.temp_max;
            e1.sensores.temp.min = e.temp_min;
            e1.sensores.hum.max = e.hum_max;
            e1.sensores.hum.min = e.hum_min;
            
            //Insercion nueva entidad completa
            string _response = data.InsertDocument("Entidades",e1.ToBsonDocument());
            
            //Actualizacion de pre-entidad marcandola como entidad activa.
            data.UpdateDocument(data.GetCollection("PreEntidades"), "activo", false, "activo", true);

            //Carga de entidad en ultimas lecturas para su mantenimiento.
            BsonDocument _eUltimaLectura = new BsonDocument();
            _eUltimaLectura.Add(new BsonElement("_id", e1.id_entidad));
            _eUltimaLectura.Add(new BsonElement("temp", 0));
            _eUltimaLectura.Add(new BsonElement("hum", 0));
            data.InsertDocument("UltimasLecturas",_eUltimaLectura);
            
            //Devolucion de respuesta con confirmacion de insercion o error encontrado.
            //REVISAR FORMATO EN QUE DEVUELVE
            return JObject.Parse("{\"result\": \"" + _response  + "\",\"_id\":\"" + e1.id_entidad.ToString() + "\"}");
        }
    }
}