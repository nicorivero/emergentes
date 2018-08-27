using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api_tecn_emergentes.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;

namespace api_tecn_emergentes.Controllers
{
    [Produces("application/json")]
    [Route("api/Entidades/[action]")]
    public class EntidadesController : Controller
    {
        private DataAccess data = new DataAccess();
        
        //Obtener toda la informacion de una entidad
        [HttpGet("id={_id_entity}")]
        public BsonDocument Get(int _id_entity)
        {
            return data.GetDocument("id_entidad", _id_entity, "Entidades");
        }

        //Eliminar una entidad
        [HttpDelete("id={_id_entity}")]
        public string Eliminar (int _id_entity)
        {
            return data.DeleteDocument("Entidades", "id_entidad", _id_entity);
        }
        //Cargar una nueva entidad
        [HttpPost]
        public string Nuevo()
        {
            Entidades e1 = new Entidades();
            return data.InsertDocument("Entidades",e1.ToBsonDocument());
        }
    }

    
}