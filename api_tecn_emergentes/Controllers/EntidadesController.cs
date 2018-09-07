using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api_tecn_emergentes.Controllers;
using api_tecn_emergentes.Models;
using api_tecn_emergentes.Auxiliar;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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
            List<JObject> _formattedList = new List<JObject>();
            foreach (BsonDocument _bdoc in _bsonlist)
            {
                JObject jdoc = JObject.Parse(_bdoc.ToJson());
                _formattedList.Add(jdoc);
            }
            return _formattedList;
        }

        //Eliminar una entidad
        [HttpDelete("id={_id_entity}")]
        public string Eliminar (int _id_entity)
        {
            return data.DeleteDocument("Entidades", "id_entidad", _id_entity);
        }
        
        //Cargar una nueva entidad
        [HttpPost]
        public string Crear([FromBody] EntidadSimple e)
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
            e1.id_entidad = _preEntElements[0].Value.ToInt32();;
            e1.nombre = e.nombre;
            //Reactores 
            e1.reactores.Add(new Reactor() { tipo = "Riego", estado = false });
            e1.reactores.Add(new Reactor() { tipo = "Climatizador", estado = false });
            //Sensores
            e1.sensores.temp.max = e.temp_max;
            e1.sensores.temp.min = e.temp_min;
            e1.sensores.hum.max = e.hum_max;
            e1.sensores.hum.min = e.hum_min;
            //Insercion nueva entidad completa
            string _response = data.InsertDocument("Entidades",e1.ToBsonDocument());
            //Actualizacion de pre-entidad marcandola como entidad activa
            data.UpdateDocument(data.GetCollection("PreEntidades"), "activo", false, "activo", true);
            //Devolucion de respuesta con confirmacion de insercion o error encontrado.
            return "{\"result\": \"" + _response  + "\",\"_id\":\"" + e1.id_entidad.ToString() + "\"}";
        }
    }

    
}