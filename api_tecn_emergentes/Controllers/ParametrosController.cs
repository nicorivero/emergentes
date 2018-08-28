using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api_tecn_emergentes.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace api_tecn_emergentes.Controllers
{
    [Produces("application/json")]
    [Route("api/sensores/Parametros/[action]")]
    public class ParametrosController : Controller
    {
        DataAccess data = new DataAccess();
       
    }
}