using System.Collections.Generic;
using api_tecn_emergentes.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Newtonsoft.Json.Linq;

namespace api_tecn_emergentes.Controllers
{
    [Produces("application/json")]
    [Route("api/Tablero/[action]")]
    public class TableroController : Controller
    {
        private DataAccess data = new DataAccess();
        private SensoresController _sensoresData = new SensoresController();
        //Obtener toda la informacion de las entidades
        [HttpGet]
        public List<JObject> Get()
        {
            List<JObject> list = new List<JObject>();
            List<BsonDocument> _bsonLecturas = data.GetDocsWithProjection("UltimasLecturas", new string[] { "_id" });
            List<JObject> _jsonLecturas = new List<JObject>();
            _bsonLecturas.ForEach(d => _jsonLecturas.Add(JObject.Parse(d.ToJson())));
            
            foreach (JObject _formatted in _jsonLecturas)
            {
                int _id_entity = int.Parse(_formatted.SelectToken("_id").ToString());
                double temp = double.Parse(_formatted.SelectToken("temp").ToString());
                double hum = double.Parse(_formatted.SelectToken("hum").ToString());

                //La coleccion Parametros no existe como tal, se integro con las entidades. Cambiar por datos de sensores en entidades
                //string _document = data.GetDocsWithProjection("Entidades", new string[] { "_id"}, "id_entidad", _id_entity).ToJson();
                //Se utilizo parametros get del controlador sensores para recuperar la info. Revisar estructura de tokens.
                JObject _doc = _sensoresData.Parametros(_id_entity);
                JToken _tokenMain = _doc.SelectToken("sensores");
                JToken _tokenTemp = _tokenMain.SelectToken("temp");
                JToken _tokenHum = _tokenMain.SelectToken("hum");

                double tempMax = double.Parse(_tokenTemp.SelectToken("max").ToString());
                double tempMin = double.Parse(_tokenTemp.SelectToken("min").ToString());
                double humMax = double.Parse(_tokenHum.SelectToken("max").ToString());
                double humMin = double.Parse(_tokenHum.SelectToken("min").ToString());

                //Calculamos el color de la temperatura
                int colorTemp = 0;
                if (temp >= tempMin && temp <= tempMax)
                    colorTemp = 2;
                else if (temp < tempMin)
                    colorTemp = 1;
                else if (temp > tempMax)
                    colorTemp = 3;

                //Calculamos el color de la humedad
                int colorHum = 0;
                if (hum >= humMin && hum <= humMax)
                    colorTemp = 2;
                else if (hum < humMin)
                    colorTemp = 4;
                else if (hum > humMax)
                    colorTemp = 5;
                //Revisar el objeto devuelto.
                JObject _objTablero = new JObject{{"id_entidad", _id_entity}, {"temperatura", temp}, {"humedad", hum}, {"colorTemp", colorTemp}, {"colorHum", colorHum}};
                list.Add(_objTablero);
            }

            return list;
        }
    }

}