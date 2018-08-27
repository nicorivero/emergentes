using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api_tecn_emergentes.Auxiliar
{
    public class auxiliar_testing
    {
        public int CalcularId()
        {
            Random rdn = new Random();
            int a = rdn.Next(100, 10000);
            var result = Int32.Parse(DateTime.Today.Day.ToString()) +
                        Int32.Parse(DateTime.Today.Minute.ToString()) * 3 +
                        Int32.Parse(DateTime.Today.Second.ToString()) * 5 + a;
            return result;
        }
        
    }
}
