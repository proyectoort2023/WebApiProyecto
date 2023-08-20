using BDTorneus;
using DTOs_Compartidos.DTOs;
using Negocio.DTOs;
using System.Drawing.Text;
using TorneusClienteWeb.Servicios_de_Datos;
using static MudBlazor.CategoryTypes;

namespace TorneusClienteWeb.Servicios
{
    public class NotificacionServicio
    {

        private readonly NotificacionServicioDatos  _notificacionServicioDatos;

        public NotificacionServicio(NotificacionServicioDatos notificacionServicioDatos)
        {
            _notificacionServicioDatos = notificacionServicioDatos;
         }

        public async Task<bool> RegistrarNotificación(string mensaje, EquipoDTO equipo, TorneoDTO torneo, bool general)
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








    }
}
