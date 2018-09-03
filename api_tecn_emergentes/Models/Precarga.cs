using System.Net;

namespace api_tecn_emergentes.Controllers
{
    public class Precarga
    {
        public int id { get; set; }
        public IPAddress ip_reactores { get; set; }
        public IPAddress ip_sensores { get; set; }
    }
}