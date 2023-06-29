﻿using Microsoft.AspNetCore.SignalR;

namespace WebApiTorneus.HubSignalR
{
    public class TorneusHub : Hub
    {

        public async Task EnviarMensajeCierreInscripciones(string torneoId)
        {
         
                await Clients.All.SendAsync("RecibidorCierreInscripciones",torneoId);


            //// Obtenemos el ID de conexión del cliente emisor
            //var connectionId = Context.ConnectionId;

            //// Obtenemos todos los clientes conectados excepto el emisor
            //var otherClients = Clients.AllExcept(connectionId);

            //// Enviamos la notificación a los clientes restantes
            //await otherClients.SendAsync("ReceiveNotification", message);
        }

        public async Task EnviarNotificaciones(string grupoDestino, string mensaje)
        {
            if (grupoDestino == "TODOS")
            {
                await Clients.All.SendAsync("RecibidorNotificaciones",mensaje);
            }
            else
            {
                await Clients.Group(grupoDestino).SendAsync("RecibidorNotificacionesEquipos", mensaje);
            }
        }


        public async Task AgregarAGrupo(string grupoDestino)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, grupoDestino);
        }

        public async Task EnviarNotificacionSuspensionTorneo(bool suspendido,int torneoId)
        {
           //// Obtenemos el ID de conexión del cliente emisor
           //// var connectionId = Context.ConnectionId;

           //// Obtenemos todos los clientes conectados excepto el emisor
           //// var otrosClientes = Clients.AllExcept(connectionId);

           //// Enviamos la notificación a los clientes restantes
           ////await otrosClientes.SendAsync("RecibidorNotficacionSuspension", suspendido, torneoId);

            await Clients.All.SendAsync("RecibidorNotficacionSuspension", suspendido, torneoId);
        }

    }
}
