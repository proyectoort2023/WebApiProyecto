using DTOs_Compartidos.DTOs;
using Negocio.DTOs;
using TorneusClienteWeb.Servicios_de_Datos;

namespace TorneusClienteWeb.Servicios
{
    public class NotificacionServicio
    {
        private readonly NotificacionServicioDatos _notificacionServicioDatos;

        private List<NotificacionDTO> Notificaciones = new();

        public NotificacionServicio(NotificacionServicioDatos notificacionServicioDatos)
        {
            _notificacionServicioDatos = notificacionServicioDatos;
        }


        public async Task SetNotificacion(NotificacionDTO notificacion)
        {
            Notificaciones.Add(notificacion);
            Notificaciones = Notificaciones.OrderByDescending(o => o.FechaHora).ToList();
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

        public async Task<List<NotificacionDTO>> ObtenerNotificaciones(UsuarioLogueado usuario)
        {
            try
            {
                if (Notificaciones.Count == 0)
                {
                    await ObtenerNotificacionesDatos(usuario);
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
