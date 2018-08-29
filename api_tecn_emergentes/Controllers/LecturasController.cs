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
            string response = data.InsertDocument("Lecturas", _lectura.ToBsonDocument());
            return response;
        }

        [HttpGet]
        public List<JObject> Consultar()
        {
            List<BsonDocument> _listaBson = new List<BsonDocument>();
            List<JObject> _formattedList = new List<JObject>();
            _listaBson = data.GetAllDocuments("Lecturas");
            foreach(BsonDocument _bdoc in _listaBson)
            {
                JObject jdoc = JObject.Parse(_bdoc.ToJson());
                _formattedList.Add(jdoc);
            }
            return _formattedList;
        }

        //[HttpGet("{id_entity}")]
        //public List<JObject> Consultar(int id_entity)
        //{
        //    //data.GetDocument("id", id_entity, "Lecturas");
        //    //return 
        //}
    }
}