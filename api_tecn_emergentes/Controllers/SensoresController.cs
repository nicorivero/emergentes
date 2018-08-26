using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using api_tecn_emergentes.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace api_tecn_emergentes.Controllers
{
    [Produces("application/json")]
    [Route("api/Sensores/[action]")]
    public class SensoresController : Controller
    {
        [HttpGet("id={_id_entity}")]
        public BsonDocument GetParameters(string _id_entity)
        {
            DataAccess data = new DataAccess();
            return data.GetDocument("id_entidad", _id_entity, "Entidades");
        }

        [HttpPost]
        public void CreateParameters()
        {

        }
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