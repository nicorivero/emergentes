using System.Collections.Generic;
using System.Linq;
using api_tecn_emergentes.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json.Linq;

namespace api_tecn_emergentes.Controllers
{
    [Produces("application/json")]
    [Route("api/Precarga/[action]")]
    public class PrecargaController : Controller
    {
        private DataAccess data = new DataAccess();
        
        //Obtener toda la informacion de una pre-entidad
        [HttpGet("id={_id_entity}")]
        public JObject Get(int _id_entity)
        {
            var document = data.GetDocsWithProjection("Pre-Entidades", new string[] { "_id"}, "id", _id_entity).First().ToJson();
            return JObject.Parse(document);
        }

        //Metodo que devuelve como resultado una lista de todas las pre-entidades dadas de alta
        [HttpGet]
        public List<JObject> Todos()
        {
            List<BsonDocument> _bsonlist = data.GetDocsWithProjection("Pre-Entidades", new string[] { "_id" });
            List<JObject> _formattedList = new List<JObject>();
            foreach (BsonDocument _bdoc in _bsonlist)
            {
                JObject jdoc = JObject.Parse(_bdoc.ToJson());
                _formattedList.Add(jdoc);
            }
            return _formattedList;
        }

        //Eliminar una pre-entidad
        [HttpDelete("id={_id_entity}")]
        public string Eliminar (int _id_entity)
        {
            return data.DeleteDocument("Pre-Entidades", "id", _id_entity);
        }
        
        //Cargar una nueva pre-entidad
        [HttpPost]
        public string Crear([FromBody] Precarga _pre)
        {
            return data.InsertDocument("Pre-Entidades", _pre.ToBsonDocument());
        }
    }
}