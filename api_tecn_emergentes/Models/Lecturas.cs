using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api_tecn_emergentes.Models
{
    public class Lecturas
    {
        public double temperatura { get; set; }
        public string fecha { get; set; }
        public int id_entidad { get; set; } 
        public double humedad {get; set;}
    }
}
