using Microsoft.AspNetCore.Mvc;
using api_tecn_emergentes.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq;
using Newtonsoft.Json.Linq;
using System.Net;

namespace api_tecn_emergentes.Controllers
{
    [Produces("application/json")]
    [Route("api/Sensores/[action]")]
    public class SensoresController : Controller
    {
        private DataAccess data = new DataAccess();

        //Obtener Parametros Cargados
        [HttpGet("id={_id_entity}")]
        public JObject Parametros(int _id_entity)
        {
            var result = data.GetDocsWithProjection("Entidades", new string[] { "_id", "reactores" }, "id_entidad", _id_entity);
            var jsonresult = Newtonsoft.Json.Linq.JObject.Parse(result.First().ToJson());
            return jsonresult;
        }
        
        //Actualizacion de Parametros de Temperatura/Humedad

        [HttpPut]
        public void SetTemperatura([FromBody] Parametros _dataToUpdate)
        {
            //Obtener datos actuales de entidad
            
            var _param = JObject.Parse(Parametros(_dataToUpdate.id_entidad).ToString()).GetValue("sensores");
            string ipAddress = _param.SelectToken("ip_sensor").ToString();
            double tmax = double.Parse(_param.SelectToken("temp.max").ToString());
            double tmin = double.Parse(_param.SelectToken("temp.min").ToString());
            double hmax = double.Parse(_param.SelectToken("hum.max").ToString());
            double hmin = double.Parse(_param.SelectToken("hum.min").ToString());

            Sensor sensorUpdate = new Sensor() { 
                ip_sensor = IPAddress.Parse(ipAddress),
                temp = new Temperature() { min = _dataToUpdate.min, max = _dataToUpdate.max},
                hum = new Humidity() { min = hmin, max = hmax} 
                };
            //Revisar Metodo Update de clase DataAccess.
            IMongoCollection<BsonDocument> collection = data.GetCollection("Entidades");
            data.UpdateDocument(collection, "id_entidad", _dataToUpdate.id_entidad, "sensores", 
                                sensorUpdate );
        }

        [HttpPut]
        public void SetHumedad([FromBody]Parametros _dataToUpdate)
        {
            var _param = JObject.Parse(Parametros(_dataToUpdate.id_entidad).ToString()).GetValue("sensores");
            string ipAddress = _param.SelectToken("ip_sensor").ToString();
            double tmax = double.Parse(_param.SelectToken("temp.max").ToString());
            double tmin = double.Parse(_param.SelectToken("temp.min").ToString());
            double hmax = double.Parse(_param.SelectToken("hum.max").ToString());
            double hmin = double.Parse(_param.SelectToken("hum.min").ToString());

            Sensor sensorUpdate = new Sensor() { 
                ip_sensor = IPAddress.Parse(ipAddress),
                temp = new Temperature() { min = tmin, max = tmax},
                hum = new Humidity() { min = _dataToUpdate.min, max = _dataToUpdate.max} 
                };
            //Revisar Metodo Update de clase DataAccess.
            IMongoCollection<BsonDocument> collection = data.GetCollection("Entidades");
            data.UpdateDocument(collection, "id_entidad", _dataToUpdate.id_entidad, "sensores", 
                                sensorUpdate );
        }
        
    }
}