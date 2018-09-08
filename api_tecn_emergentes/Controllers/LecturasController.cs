using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api_tecn_emergentes.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace api_tecn_emergentes.Controllers
{
    [Produces("application/json")]
    [Route("api/entidades/lecturas/[action]")]
    public class LecturasController : Controller
    {
        DataAccess data = new DataAccess();
        Models.RabbitMQ rq = new Models.RabbitMQ();

        [HttpPost]
        
        public string Insertar([FromBody] Lecturas _lectura)
        {
            //Update UltimasLecturas
            
            //Activar/Desactivar Riego/Ventailacion
            BsonDocument _docEntidad = data.GetDocsWithProjection("Entidades", new string[]{"_id"}, "id_entidad", _lectura.id_entidad).First();
            List<BsonElement> lista =_docEntidad.Elements.ToList();
            //VARIABLES MOCK PARA TESTING
            double tmax = 30.5;
            double tmin = 27.2;
            double hmax = 75.7;
            double hmin = 52.2;
            // bool _clima = _lectura.temperatura > tmax? true:_lectura.temperatura < tmin? false:;
            // bool _riego = _lectura.humedad < hmin? true:_lectura.humedad>hmax?false;
            // rq.PostMessage(JsonConvert.SerializeObject(new PushData(){
            //     id_entidad=_lectura.id_entidad, riego=_riego, ventilacion=_clima}),"message");

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