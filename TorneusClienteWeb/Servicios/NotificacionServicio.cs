<<<<<<< HEAD
﻿using BDTorneus;
using DTOs_Compartidos.DTOs;
using Negocio.DTOs;
using System.Drawing.Text;
using TorneusClienteWeb.Servicios_de_Datos;
using static MudBlazor.CategoryTypes;
=======
﻿using DTOs_Compartidos.DTOs;
using Negocio.DTOs;
using TorneusClienteWeb.Servicios_de_Datos;
>>>>>>> dev: front notificacion estructura

namespace TorneusClienteWeb.Servicios
{
    public class NotificacionServicio
    {
<<<<<<< HEAD

        private readonly NotificacionServicioDatos  _notificacionServicioDatos;
=======
        private readonly NotificacionServicioDatos _notificacionServicioDatos;
>>>>>>> dev: front notificacion estructura

        private List<NotificacionDTO> Notificaciones = new();

        public NotificacionServicio(NotificacionServicioDatos notificacionServicioDatos)
        {
            _notificacionServicioDatos = notificacionServicioDatos;
<<<<<<< HEAD
         }
=======
        }
>>>>>>> dev: front notificacion estructura


        public async Task SetNotificacion(NotificacionDTO notificacion)
        {
            Notificaciones.Add(notificacion);
            Notificaciones = Notificaciones.OrderByDescending(o => o.FechaHora).ToList();
        }

        public async Task<bool> RegistrarNotificacion(string mensaje, EquipoDTO equipo, TorneoDTO torneo, bool general)
<<<<<<< HEAD
        { 
            try 
	            {	  
=======
        {
            try
            {
>>>>>>> dev: front notificacion estructura
                NotificacionDTO notificacion = new NotificacionDTO()
                {
                    Mensaje = mensaje,
                    Equipo = equipo,
                    Torneo = torneo,
                    General = general
                };
<<<<<<< HEAD
                    bool registrado = await _notificacionServicioDatos.RegistrarNotificacion(notificacion);
                 return registrado;
            }
	        catch (Exception ex)
	        {
                    throw new Exception(ex.Message);
	        }
        }

        public async Task<List<NotificacionDTO>> ObtenerNotificaciones(int usuarioId)
=======
                bool registrado = await _notificacionServicioDatos.RegistrarNotificacion(notificacion);
                return registrado;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<NotificacionDTO>> ObtenerNotificaciones(UsuarioLogueado usuario)
>>>>>>> dev: front notificacion estructura
        {
            try
            {
                if (Notificaciones.Count == 0)
                {
<<<<<<< HEAD
                    await ObtenerNotificacionesDatos(usuarioId);
=======
                    await ObtenerNotificacionesDatos(usuario);
>>>>>>> dev: front notificacion estructura
                }
                return Notificaciones;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


<<<<<<< HEAD
        private async Task ObtenerNotificacionesDatos(int usuarioId)
        {
            try
            {
                Notificaciones = await _notificacionServicioDatos.ObtenerListadoNotificaciones(usuarioId);
=======
        private async Task ObtenerNotificacionesDatos(UsuarioLogueado usuario)
        {
            try
            {
                Notificaciones = await _notificacionServicioDatos.ObtenerListadoNotificaciones(usuario);
>>>>>>> dev: front notificacion estructura
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

<<<<<<< HEAD






=======
>>>>>>> dev: front notificacion estructura
    }
}
