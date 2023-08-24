using BDTorneus;
using DTOs_Compartidos.DTOs;
using Microsoft.AspNetCore.Components;
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

        private List<NotificacionDTO> Notificaciones = new();

        private bool cargadasNotificaciones = false;

        public NotificacionServicio(NotificacionServicioDatos notificacionServicioDatos, UsuarioServicio usuarioServicio)
        {
            _notificacionServicioDatos = notificacionServicioDatos;
            _usuarioServicio = usuarioServicio;
        }


        public async Task SetNotificacion(NotificacionDTO notificacion)
        {
            if (!cargadasNotificaciones)
            {
                await ObtenerNotificacionesDatos(_usuarioServicio.ObtenerUsuarioLogueado());
            }
            else
            {
                Notificaciones.Add(notificacion);
                Notificaciones = Notificaciones.OrderByDescending(o => o.FechaHora).ToList();
            }
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
                bool registrado = await _notificacionServicioDatos.RegistrarNotificacion(notificacion);
                return registrado;
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
                if (cargadasNotificaciones)
                {
                    await ObtenerNotificacionesDatos(_usuarioServicio.ObtenerUsuarioLogueado());
                }
                return Notificaciones;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        private async Task ObtenerNotificacionesDatos(UsuarioLogueado usuario)
        {
            try
            {
                Notificaciones = await _notificacionServicioDatos.ObtenerListadoNotificaciones(usuario);
                Notificaciones = Notificaciones.OrderByDescending(o => o.FechaHora).ToList();
                cargadasNotificaciones = true;
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
