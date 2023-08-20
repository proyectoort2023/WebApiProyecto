﻿using BDTorneus;
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

        public async Task<List<NotificacionDTO>> ObtenerNotificaciones(int usuarioId)
        {
            try
            {
                if (Notificaciones.Count == 0)
                {
                    await ObtenerNotificacionesDatos(usuarioId);
                }
                return Notificaciones;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        private async Task ObtenerNotificacionesDatos(int usuarioId)
        {
            try
            {
                Notificaciones = await _notificacionServicioDatos.ObtenerListadoNotificaciones(usuarioId);
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
