using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api_tecn_emergentes.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Newtonsoft.Json.Linq;

namespace api_tecn_emergentes.Controllers
{
    [Produces("application/json")]
    [Route("api/entidades/lecturas/[action]")]
    public class LecturasController : Controller
    {
        DataAccess data = new DataAccess();

        [HttpPost]
        
        public string Insertar([FromBody] Lecturas _lectura)
        {
            //Update Ultimas-Lecturas
            
            //Activar/Desactivar Riego/Ventailacion
            BsonDocument _docEntidad = data.GetDocsWithProjection("Entidades", new string[]{"_id"}, "id_entidad", _lectura.id_entidad).First();
            List<BsonElement> lista =_docEntidad.Elements.ToList();
            
            //Grabar Lectura en Historico
            string response = data.InsertDocument("Lecturas", _lectura.ToBsonDocument());
            return response;
        }

        [HttpGet]
        public List<JObject> Consultar()
        {
            List<BsonDocument> _listaBson = new List<BsonDocument>();
            List<JObject> _formattedList = new List<JObject>();
            _listaBson = data.GetDocsWithProjection("Lecturas", new string[]{"_id"});
            foreach(BsonDocument _bdoc in _listaBson)
            {
                JObject jdoc = JObject.Parse(_bdoc.ToJson());
                _formattedList.Add(jdoc);
            }
            return _formattedList;
        }

        [HttpGet("{id_entity}")]
        public List<JObject> Consultar(int id_entity)
        {
            List<BsonDocument> _listaBson = data.GetDocsWithProjection("Lecturas", new string[]{"_id"}, "id_entidad", id_entity);
            List<JObject> _formattedList = new List<JObject>();
            foreach(BsonDocument _bdoc in _listaBson)
            {
                JObject jdoc = JObject.Parse(_bdoc.ToJson());
                _formattedList.Add(jdoc);
            }
            return _formattedList;
        }
    }
}