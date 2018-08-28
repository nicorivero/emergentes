using Microsoft.AspNetCore.Mvc;
using api_tecn_emergentes.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq;

namespace api_tecn_emergentes.Controllers
{
    [Produces("application/json")]
    [Route("api/Sensores/[action]")]
    public class SensoresController : Controller
    {
        private DataAccess data = new DataAccess();

        //Obtener Parametros Cargados
        [HttpGet("id={_id_entity}")]
        public Newtonsoft.Json.Linq.JObject Parametros(int _id_entity)
        {
            var result = data.GetDocsWithProjection("Entidades", new string[] { "_id", "reactores" }, "id_entidad", _id_entity);
            var jsonresult = Newtonsoft.Json.Linq.JObject.Parse(result.First().ToJson());
            return jsonresult;
        }
        //Actualizacion de Parametros de Temperatura/Humedad
        #region UpdateParameters
        [HttpPut("id={id_entity}+tmax={temp_max}+tmin={temp_min}")]
        [Route("api/sensores/temperatura/update")]
        public void Temperatura(int id_entity, double tmax, double tmin)
        {
            //Revisar Metodo Update de clase DataAccess.
        }
        [HttpPut("id={id_entity}+hmax={hum_max}+hmin={hum_min}")]
        [Route("api/sensores/humedad/update")]
        public void ActualizarHumedad(int id_entity, double hmax, double hmin)
        {
            //Revisar Metodo Update de clase DataAccess.
        }
        #endregion
    }
}