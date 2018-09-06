using System;
using System.Collections.Generic;
using api_tecn_emergentes.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace api_tecn_emergentes.Controllers
{
    [Produces("application/json")]
    [Route("api/Tablero/[action]")]
    public class TableroController : Controller
    {
        private DataAccess data = new DataAccess();
        
        //Obtener toda la informacion de una entidad
        [HttpGet]
        public JObject Get()
        {
            List<BsonDocument> _bsonLecturas = data.GetDocsWithProjection("Ultimas-Lecturas", new string[] { "_id" });
            List<JObject> _formattedLecturas = new List<JObject>();
            foreach (BsonDocument _bdoc in _bsonLecturas)
            {
                JObject jdoc = JObject.Parse(_bdoc.ToJson());
                _formattedLecturas.Add(jdoc);
            }
            
            foreach (JObject _formatted in _formattedLecturas)
            {
                int _id_entity = _formatted.SelectToken("id_entidad");
                double temp = _formatted.SelectToken("temperatura");
                double hum = _formatted.SelectToken("humedad");

                var _document = data.GetDocsWithProjection("Parametros", new string[] { "_id"}, "id_entidad", _id_entity).First().ToJson();
                JObject _doc = JObject.Parse(_document);

                double tempMax = _doc.SelectToken("tempMax");
                double tempMin = _doc.SelectToken("tempMin");
                double humMax = _doc.SelectToken("humMax");
                double humMin = _doc.SelectToken("humMin");

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

                // id_entidad, datos de sensores, color 
            }
        }
    }

}