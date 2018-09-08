using System;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using api_tecn_emergentes.Models;
using Newtonsoft.Json;

namespace api_tecn_emergentes.Controllers
{
    [Produces("application/json")]
    [Route("api/Reactores/[action]")]
    public class ReactoresController : Controller
    {
        DataAccess data = new DataAccess();
        api_tecn_emergentes.Models.RabbitMQ rq = new api_tecn_emergentes.Models.RabbitMQ();

        //Metodo de activacion, envia JSON a placas a traves de un channel RabbitMQ
        private void PushActivar(PushData _data, string _queue)
        { 
            string _jsonToSend = JsonConvert.SerializeObject(_data);
            rq.PostMessage(_jsonToSend,_queue); 
        }
        
        //Llamada para activar/desactivar reactores desde front end
        [HttpPost()]
        public JObject Manual([FromBody] PushData _data)
        {
            try
            {
                PushActivar(_data,"message");
                return JObject.Parse(JsonConvert.SerializeObject("{\"estado\":\"OK\",\"mensaje\":\"notificacion enviada correctamente\"}"));
            }
            catch (Exception ex)
            {
                return JObject.Parse(JsonConvert.SerializeObject(ex));
            }
        }
    }
}