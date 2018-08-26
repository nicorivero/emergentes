using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using api_tecn_emergentes.Models;

namespace api_tecn_emergentes.Controllers
{
    [Produces("application/json")]
    [Route("api/Sensores/[action]")]
    public class SensoresController : Controller
    {
        [HttpGet("{id_entity}")]
        public void GetParameters(string id_entity)
        {
            DataAccess data = new DataAccess();

            
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