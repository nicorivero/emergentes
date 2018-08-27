using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
        public BsonDocument GetParameters(int _id_entity)
        {
            var result = data.GetDocsWithProjection("Entidades", new string[] { "_id", "reactores" }, "id_entidad", _id_entity);
            return result.First();
        }
        //Actualizacion de Parametros de Temperatura/Humedad
        #region UpdateParameters
        [HttpPut]
        public void UpdateTemp()
        {

        }
        [HttpPut]
        public void UpdateHum()
        {

        }
        #endregion
    }
}