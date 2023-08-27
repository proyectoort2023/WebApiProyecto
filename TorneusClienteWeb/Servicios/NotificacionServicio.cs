using BDTorneus;
using DTOs_Compartidos.DTOs;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Negocio.DTOs;
using System.Drawing.Text;
using TorneusClienteWeb.Servicios_de_Datos;
using static MudBlazor.CategoryTypes;

namespace TorneusClienteWeb.Servicios
{
    public class NotificacionServicio
    {
        private readonly NotificacionServicioDatos _notificacionServicioDatos;
        [Inject] private UsuarioServicio _usuarioServicio { get; set; }
        [Inject] private HubConnection _hubConnection { get; set; }

        private List<NotificacionDTO> Notificaciones = new();

        private bool cargadasNotificaciones = false;

        public NotificacionServicio(NotificacionServicioDatos notificacionServicioDatos, UsuarioServicio usuarioServicio, HubConnection hubConnection)
        {
            _notificacionServicioDatos = notificacionServicioDatos;
            _usuarioServicio = usuarioServicio;
            _hubConnection = hubConnection;
        }

        public async Task<bool> RegistrarNotificacion(string mensaje, EquipoDTO equipo, TorneoDTO torneo, bool general)
        {
            try
            {
                NotificacionDTO notificacion = new NotificacionDTO()
                {
                    Mensaje = mensaje,
                    Equipo = equipo,
                    Torneo = torneo,
                    General = general
                };
                NotificacionDTO registrado = await _notificacionServicioDatos.RegistrarNotificacion(notificacion);

                await _hubConnection.SendAsync("EnviarNuevaNotificacion", registrado);
                return registrado != null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task<List<NotificacionDTO>> ObtenerNotificaciones()
        {
            try
            {
                UsuarioLogueado usuario = _usuarioServicio.ObtenerUsuarioLogueado();
                Notificaciones = await _notificacionServicioDatos.ObtenerListadoNotificaciones(usuario);
                Notificaciones = Notificaciones.OrderByDescending(o => o.FechaHora).ToList();
                return Notificaciones;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task<bool> BorrarNotificacionesDatos(int torneoId)
        {
            try
            {
                bool borrados = await _notificacionServicioDatos.BorrarNotificacionesFinTorneo(torneoId);
                return borrados;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }




    }
}
