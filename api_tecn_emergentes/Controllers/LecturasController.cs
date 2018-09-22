using System.Collections.Generic;
using System.Linq;
using api_tecn_emergentes.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
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
            //Si no existe la entidad no deberia poder grabar lecturas
            if (data.GetDocument("id",_lectura.id_entidad,"Entidades").Elements.ToList()[0].Name == "_code")
            {
                return JObject.Parse("{\"return\":\"No se pueden anexar lecturas de una entidad no registrada\"}").ToString();
            }

            //Update UltimasLecturas
            IMongoCollection<BsonDocument> collection = data.GetCollection("UltimasLecturas");
            data.UpdateDocument(collection,"_id", _lectura.id_entidad, "temp", _lectura.temperatura);
            data.UpdateDocument(collection,"_id", _lectura.id_entidad, "hum", _lectura.humedad);

            //Activar/Desactivar Riego/Ventailacion
            JObject _entidad = JObject.Parse(data.GetDocsWithProjection("Entidades", new string[]{"_id"}, "id_entidad", _lectura.id_entidad).First().ToJson());
            
            SensoresController _sense_data = new SensoresController();
            var _param = JObject.Parse(_sense_data.Parametros(_lectura.id_entidad).ToString()).GetValue("sensores");
            double tmax = double.Parse(_param.SelectToken("temp.max").ToString());
            double tmin = double.Parse(_param.SelectToken("temp.min").ToString());
            double hmax = double.Parse(_param.SelectToken("hum.max").ToString());
            double hmin = double.Parse(_param.SelectToken("hum.min").ToString());
            
            //Obtener parametros actuales de entidad
            JToken _reactoresEntidad = _entidad.GetValue("reactores");
            bool _riegoCurrentState = bool.Parse(JObject.Parse(_reactoresEntidad.First().ToString()).GetValue("estado").ToString());
            bool _ventilacionCurrentState = bool.Parse(JObject.Parse(_reactoresEntidad.Last().ToString()).GetValue("estado").ToString());

            //Parametros para mensaje a placas
            bool _clima = _lectura.temperatura > tmax? true : _lectura.temperatura < tmin? false : _ventilacionCurrentState;
            bool _riego = _lectura.humedad < hmin? true : _lectura.humedad>hmax? false : _riegoCurrentState;
            
            //Enviar mensaje a entidad fisica
            rq.PostMessage(JsonConvert.SerializeObject(new PushData(){  id_entidad=_lectura.id_entidad, 
                                                                        riego = _riego, 
                                                                        ventilacion = _clima }),"message");
            //Actualizar estado actual de reactores en entidad
            
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