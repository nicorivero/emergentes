using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace Emergentes.Controllers
{
    [Route("api/[controller]/[action]")]
    public class ClimaController : Controller
    {
        //Tratamiento de las respuestas para devolucion de datos
        private JObject responseTreatment(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
            {
                return JObject.Parse(response.Content.ReadAsStringAsync().Result);
            }
            else
            {
                JObject responseResult = new JObject();
                JToken token = "Errores";
                responseResult.Add(response.StatusCode.ToString(), token);
                responseResult.Add(response.ReasonPhrase, token);
                return responseResult;
            }
        }

        [HttpGet("{cityId}")]
        public JObject GetById(string cityId)
        {
            using (var cliente = new HttpClient())
            {
                //var cityId = "3435907";
                var apiKey = "72579e97bc01e49ecdcb363d31cce418";
                cliente.BaseAddress = new Uri("https://api.openweathermap.org");
                cliente.DefaultRequestHeaders.Accept.Clear();
                cliente.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                var response = cliente.GetAsync("data/2.5/forecast?id=" + cityId + "&APPID=" + apiKey + "&units=metric&lang=es").Result;
                var result = responseTreatment(response);
                return result;
            }
        }
        [HttpGet("{cityName}")]
        public JObject GetByName(string cityName)
        {
            using (var cliente = new HttpClient())
            {
                var apiKey = "72579e97bc01e49ecdcb363d31cce418";
                cliente.BaseAddress = new Uri("https://api.openweathermap.org");
                cliente.DefaultRequestHeaders.Accept.Clear();
                cliente.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                var response = cliente.GetAsync("data/2.5/weather?q=" + cityName + "&APPID=" + apiKey + "&units=metric&lang=es").Result;
                var result = responseTreatment(response);
                return result;
            }
        }
        [HttpGet("{lat},{lon}")]
        public JObject GetByCoordinates(string lat, string lon)
        {
            using (var cliente = new HttpClient())
            {
                var apiKey = "72579e97bc01e49ecdcb363d31cce418";
                cliente.BaseAddress = new Uri("https://api.openweathermap.org");
                cliente.DefaultRequestHeaders.Accept.Clear();
                cliente.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                var response = cliente.GetAsync("data/2.5/weather?lat=" + lat + "&lon=" + lon + "&APPID=" + apiKey + "&units=metric&lang=es").Result;
                var result = responseTreatment(response);
                return result;
            }
        }   
    }
}