using System.Net;

namespace api_tecn_emergentes.Controllers
{
    public class Precarga
    {
        public int id { get; set; }
        public string ip_reactores { get; set; }
        public string ip_sensores { get; set; }
    }
}