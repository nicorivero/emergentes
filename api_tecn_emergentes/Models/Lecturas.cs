﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api_tecn_emergentes.Models
{
    public class Lecturas
    {
        public int id_entidad { get; set; }
        public DateTime fecha { get; set; }
        public double temperatura { get; set; }
        public double humedad {get; set;}
    }
}