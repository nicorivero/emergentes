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
            List<BsonDocument> _bsonLecturas = data.GetAllDocuments("UltimasLecturas");
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
                JToken _param = JObject.Parse(_sensoresData.Parametros(_id_entity).ToString()).GetValue("sensores");
                
                double tempMax = double.Parse(_param.SelectToken("temp.max").ToString());
                double tempMin = double.Parse(_param.SelectToken("temp.min").ToString());
                double humMax = double.Parse(_param.SelectToken("hum.max").ToString());
                double humMin = double.Parse(_param.SelectToken("hum.min").ToString());

                //REVISAR ESTO!
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
                    colorHum = 2;
                else if (hum < humMin)
                    colorHum = 4;
                else if (hum > humMax)
                    colorHum = 5;
                //Revisar el objeto devuelto.
                JObject _objTablero = new JObject{{"id_entidad", _id_entity}, {"temperatura", temp}, {"humedad", hum}, {"colorTemp", colorTemp}, {"colorHum", colorHum}};
                list.Add(_objTablero);
            }

            return list;
        }
    }

}