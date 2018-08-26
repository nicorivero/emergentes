using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api_tecn_emergentes.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace api_tecn_emergentes.Controllers
{
    [Produces("application/json")]
    [Route("api/Entidades/[action]")]
    public class EntidadesController : Controller
    {
        [HttpGet("id={_id_entity}")]
        public BsonDocument Get(string _id_entity)
        {
            DataAccess data = new DataAccess();
            return data.GetDocument("id_entidad", _id_entity, "Entidades");
        }
    }

    
}