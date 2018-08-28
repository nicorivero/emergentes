using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using api_tecn_emergentes.Auxiliar;
using Newtonsoft.Json.Linq;
using api_tecn_emergentes.Models;

namespace api_tecn_emergentes.Controllers
{
    [Produces("application/json")]
    [Route("api/Reactores/[action]")]
    public class ReactoresController : Controller
    {
        signalr_hub hub = new signalr_hub();
        DataAccess data = new DataAccess();

        //Inicialmente la notificacion se distribuye a todos los clientes y la toma quien precisa
        private async void PushActivaRiego(bool _value)
        {
            if (_value) { await hub.EnviarMsj("TODOS", "{\"reactor\":\"riego\",\"valor\":true}"); }
            else { await hub.EnviarMsj("TODOS", "{\"reactor\":\"riego\",\"valor\":false}"); }
        }

        private async void PushActivaVentilacion(bool _value)
        {
            if (_value) { await hub.EnviarMsj("TODOS", "{\"reactor\":\"climatizacion\",\"valor\":true}"); }
            else { await hub.EnviarMsj("TODOS", "{\"reactor\":\"climatizacion\",\"valor\":false}"); }
        }
        //Llamada para activar/desactivar riego desde front end
        [HttpPost("id={_id_entity}+valor={_value}")]
        public JObject Riego(int _id_entity, bool _value)
        {
            PushActivaRiego(_value);
            return _value?JObject.Parse("{\"reactor\":\"riego\",\"valor\":true}"):JObject.Parse("{\"reactor\":\"riego\",\"valor\":false}");
        }
        //Llamada para activar/desactivar ventilacion desde front end
        [HttpPost("id={_id_entity}+valor={_value}")]
        public JObject Ventilacion(int _id_entity, bool _value)
        {
            PushActivaVentilacion(_value);
            return _value ? JObject.Parse("{\"reactor\":\"climatizacion\",\"valor\":true}") : JObject.Parse("{\"reactor\":\"climatizacion\",\"valor\":false}");
        }
    }
}