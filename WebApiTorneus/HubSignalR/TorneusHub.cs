using Microsoft.AspNetCore.SignalR;

namespace WebApiTorneus.HubSignalR
{
    public class TorneusHub : Hub
    {

        public async Task EnviarMensajeCierreInscripciones(string torneoId)
        {
         
                await Clients.All.SendAsync("RecibidorCierreInscripciones",torneoId);
        }

        public async Task EnviarNotificaciones(string grupoDestino, string mensaje)
        {
            if (grupoDestino == "TODOS")
            {
                await Clients.All.SendAsync("RecibidorNotificaciones",mensaje);
            }
            else
            {
                await Clients.Group(grupoDestino).SendAsync("RecibidorNotificaciones", mensaje);
            }
        }


        public async Task AgregarAGrupo(string grupoDestino)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, grupoDestino);
        }

    }
}
