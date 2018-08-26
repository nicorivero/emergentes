using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;
namespace api_tecn_emergentes.Auxiliar
{
    public class signalr_hub:Hub
    {
        public async Task EnviarMsj(string usr, string msj)
        {
            await Clients.All.SendAsync("Mensaje Recibido",usr,msj);
        }
    }
}
