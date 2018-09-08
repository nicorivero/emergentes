using Microsoft.AspNetCore.Mvc;
using api_tecn_emergentes.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq;
using Newtonsoft.Json.Linq;

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

        #region UpdateParameters

        [HttpPut("id={id_entity}/tmax={tmax}/tmin={tmin}")]
        public void SetTemperatura(int id_entity ,double tmax, double tmin)
        {
            //Revisar Metodo Update de clase DataAccess.
            IMongoCollection<BsonDocument> collection = data.GetCollection("Entidades");
            data.UpdateDocument(collection, "id_entidad", id_entity, "temp", new Temperature() { min=tmin,max=tmax } );
        }

        [HttpPut("id={id_entity}/hmax={hmax}/hmin={hmin}")]
        public void SetHumedad(int id_entity, double hmax, double hmin)
        {
            IMongoCollection<BsonDocument> collection = data.GetCollection("Entidades");
            data.UpdateDocument(collection, "id_entidad", id_entity, "hum", new Humidity() { min = hmin, max = hmax });
        }
        #endregion
    }
}