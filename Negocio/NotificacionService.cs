using BDTorneus;
using Microsoft.EntityFrameworkCore;
using Negocio.DTOs;
using Negocio.Validaciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilidades;

namespace Negocio
{
    public class NotificacionService
    {
        private readonly TorneoContext _db;
        private readonly InscripcionService _inscripcionService;
        private readonly UsuarioService _usuarioService;

        public NotificacionService(TorneoContext db, InscripcionService inscripcionService, UsuarioService usuarioService)
        {
            _db = db;
            _inscripcionService = inscripcionService;
            _usuarioService = usuarioService;
        }

        public async Task<Notificacion> Registrar(Notificacion notificacion)
        {
            try
            {
                if (notificacion == null) throw new Exception("No hay notificacion para registrar");

                notificacion.FechaHora = DateTime.Now;

                _db.Entry(notificacion.Torneo).State = EntityState.Unchanged;
                _db.Entry(notificacion.Equipo).State = EntityState.Unchanged;

                var registrado = await _db.Notificaciones.AddAsync(notificacion);
                await _db.SaveChangesAsync();

                return registrado.Entity;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }



        public async Task<List<Notificacion>> ObtenerSegunUsuario(int usuarioId)
        {
            Usuario usuario = await _usuarioService.ObtenerUsuarioSegunId(usuarioId);

            List<Notificacion> notificaciones = new List<Notificacion>();

            try
            {
                if (usuario.Rol != Util.Roles.EQUIPO.ToString())
                {
                    notificaciones = await _db.Notificaciones.Include(i => i.Torneo)
                                                             .Include(i => i.Equipo)
                                                             .Where(w => w.General == true)
                                                             .OrderByDescending(o => o.FechaHora)
                                                             .ToListAsync();
                    return notificaciones;
                 }
                else
                {
                    var inscrpciones = await _inscripcionService.ObtenerInscripcionesSegunUsuario(usuario.Id);
                    List<int> listaIdTorneos = new();

                    foreach(var inscripcion in inscrpciones)
                    {
                        listaIdTorneos.Add(inscripcion.Torneo.Id);
                    }

                    if (listaIdTorneos.Count > 0)
                    {
                        notificaciones = await _db.Notificaciones.Include(i => i.Torneo)
                                                                 .Include(i => i.Equipo)
                                                                 .Where(w => listaIdTorneos.Contains(w.Torneo.Id))
                                                                 .OrderByDescending(o => o.FechaHora)
                                                                 .ToListAsync();
                        return notificaciones;
                    }
                    else
                    {
                        return notificaciones;
                    }

                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }



        public async Task<bool> BorrarNotificacionesTerminoPartido(int torneoId)
        {
            try
            {
                var notificacionesEliminar = await _db.Notificaciones.Where(w => w.Torneo.Id == torneoId).ToListAsync();
                _db.Notificaciones.RemoveRange(notificacionesEliminar);
                int cantidadBorradas = await _db.SaveChangesAsync();
                return cantidadBorradas > 0;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }



    }
}
