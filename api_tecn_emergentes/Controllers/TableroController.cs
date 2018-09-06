using System;
using System.Collections.Generic;
using api_tecn_emergentes.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json.Linq;

namespace api_tecn_emergentes.Controllers
{
    [Produces("application/json")]
    [Route("api/Tablero/[action]")]
    public class TableroController : Controller
    {
        private DataAccess data = new DataAccess();
        
        //Obtener toda la informacion de una entidad
        [HttpGet]
        public List<JObject> Get()
        {
            List<JObject> list = new List<JObject>();
            
            List<BsonDocument> _bsonLecturas = data.GetDocsWithProjection("Ultimas-Lecturas", new string[] { "_id" });
            List<JObject> _formattedLecturas = new List<JObject>();
            foreach (BsonDocument _bdoc in _bsonLecturas)
            {
                JObject jdoc = JObject.Parse(_bdoc.ToJson());
                _formattedLecturas.Add(jdoc);
            }
            
            foreach (JObject _formatted in _formattedLecturas)
            {
                int _id_entity = int.Parse(_formatted.SelectToken("id_entidad").ToString());
                double temp = double.Parse(_formatted.SelectToken("temperatura").ToString());
                double hum = double.Parse(_formatted.SelectToken("humedad").ToString());

                var _document = data.GetDocsWithProjection("Parametros", new string[] { "_id"}, "id_entidad", _id_entity).ToJson();
                JObject _doc = JObject.Parse(_document);

                double tempMax = double.Parse(_doc.SelectToken("tempMax").ToString());
                double tempMin = double.Parse(_doc.SelectToken("tempMin").ToString());
                double humMax = double.Parse(_doc.SelectToken("humMax").ToString());
                double humMin = double.Parse(_doc.SelectToken("humMin").ToString());

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

                JObject objeto = new JObject{{"id_entidad", _id_entity}, {"temperatura", temp}, {"humedad", hum}, {"colorTemp", colorTemp}, {"colorHum", colorHum}};
                list.Add(objeto);
            }

            return list;
        }
    }

}