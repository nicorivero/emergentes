using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api_tecn_emergentes.Models
{
    public class EntidadSimple
    {
        public int id_precarga { get; set; }
        public string nombre { get; set; }
        public double temp_min { get; set; }
        public double temp_max { get; set; }
        public double hum_min { get; set; }
        public double hum_max { get; set; }
    }
}
