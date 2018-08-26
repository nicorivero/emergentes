using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using api_tecn_emergentes.Auxiliar;

namespace api_tecn_emergentes.Controllers
{
    [Produces("application/json")]
    [Route("api/Reactores")]
    public class ReactoresController : Controller
    {
        signalr_hub hub = new signalr_hub();
        private async void PushActivaRiego()
        {
            await hub.EnviarMsj("1" ,"Activar Riego");
        }

        private async void PushActivaVentilacion()
        {
            await hub.EnviarMsj("1","Activar Climatizacion");
        }
    }
}