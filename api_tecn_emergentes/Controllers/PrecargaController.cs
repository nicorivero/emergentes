using System.Collections.Generic;
using System.Linq;
using api_tecn_emergentes.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json.Linq;

namespace api_tecn_emergentes.Controllers
{
    [Produces("application/json")]
    [Route("api/Entidades/Precarga/[action]")]
    public class PrecargaController : Controller
    {
        private DataAccess data = new DataAccess();
        
        //Obtener toda la informacion de una pre-entidad
        [HttpGet("id={_id_entity}")]
        public JObject Get(int _id_entity)
        {
            var document = data.GetDocsWithProjection("Pre-Entidades", new string[] { "_id"}, "id", _id_entity).First().ToJson();
            return JObject.Parse(document);
        }

        //Metodo que devuelve como resultado una lista de todas las pre-entidades pendientes de alta
        [HttpGet]
        public List<JObject> Todos()
        {
            List<BsonDocument> _bsonlist = data.GetDocumentsWithFilter("PreEntidades", "activo", false);
            List<JObject> _formattedList = new List<JObject>();
            foreach (BsonDocument _bdoc in _bsonlist)
            {
                JObject jdoc = JObject.Parse(_bdoc.ToJson());
                _formattedList.Add(jdoc);
            }
            return _formattedList;
        }

        //Eliminar una pre-entidad --Se cambia por el agregado de un campo de activacion con valor inicial false,
        //con esto las placas no cargaran nuevamente la info al iniciar pues el registro ya existira,
        //y por otro lado el frontend solo podra traer aquellas cuyo estado de activacion sea false, es decir
        //pendientes de carga.
          
        //Cargar una nueva pre-entidad (Funcionando y probado).
        [HttpPost]
        public string Crear([FromBody] Precarga _pre)
        {
            var _value = data.GetDocument("id_entidad", _pre.id, "PreEntidades").Elements.ToList();
            if (_value[0].Name == "code")
            {
                BsonElement _state = new BsonElement("activo",false);
                var _result = _pre.ToBsonDocument().Add(_state);
                return data.InsertDocument("PreEntidades", _result);
            }
            else
            {
                return "El objeto ya existe en la base de datos, se ignorara la solicitud";
            }
        }
    }
}