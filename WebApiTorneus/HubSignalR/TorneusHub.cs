using BDTorneus;
using DTOs_Compartidos.DTOs;
using DTOs_Compartidos.Models;
using Microsoft.AspNetCore.SignalR;
using Negocio.DTOs;
using WebApiTorneus.Services;

namespace WebApiTorneus.HubSignalR
{
    public class TorneusHub : Hub
    {
        private readonly FixtureTiempoReal _fixtureTiempoReal;

        public TorneusHub(FixtureTiempoReal fixtureTiempoReal)
        {
            _fixtureTiempoReal = fixtureTiempoReal;
        }

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
            await Clients.All.SendAsync("RecibidorNotficacionSuspension", suspendido, torneoId);
        }

        public async Task EnviarNotificacionEliminacionTorneo(int torneoId)
        {
            await Clients.All.SendAsync("RecibidorNotficacionEliminacion", torneoId);
        }

        public async Task EnviarNotificacionModificacionTorneo(TorneoDTO torneoDTO)
        {
            await Clients.All.SendAsync("RecibidorNotficacionModificacion", torneoDTO);
        }

        public async Task EnviarAperturaCierreTorneo(int torneoId, bool apertura)
        {
            await Clients.All.SendAsync("RecibidorAperturaCierreTorneo", torneoId, apertura);
        }

        public async Task EnviarNotificacionMercadoPago(WebHook webHook)
        {
            await Clients.All.SendAsync("RecibidorNotificacionMercadoPago", webHook);
        }

        public async Task EnviarActualizarPartidos(List<PartidoDTO> partidos)
        {
            await _fixtureTiempoReal.ActualizarPartidos(partidos);
            await Clients.All.SendAsync("RecibirActualizarPartidos", partidos);
        }

        public async Task EnviarNuevaNotificacion(NotificacionDTO notificacion)
        {
            await Clients.All.SendAsync("RecibirNuevaNotificacion", notificacion);
        }




    }
}
