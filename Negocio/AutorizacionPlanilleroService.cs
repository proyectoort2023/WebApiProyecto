using BDTorneus;
using DTOs_Compartidos.DTOs;
using DTOs_Compartidos.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilidades;

namespace Negocio
{
    public class AutorizacionPlanilleroService
    {
        private readonly TorneoContext _db;

        public AutorizacionPlanilleroService(TorneoContext db)
        {
            _db = db;
        }


        public async Task<int> AgregarPlanillleroAutorizado(AutorizacionPlanilleroDTO autPlanilleroDTO)
        {
            try
            {
                if (autPlanilleroDTO == null) throw new Exception("No existe usuario para autorizar");

                if (await ValidarPlanillero(autPlanilleroDTO)) throw new Exception("El usuario que quiere autorizar ya está autorizado en otro torneo el mismo dia");

                AutorizacionPlanillero autorizacionPlanillero = new AutorizacionPlanillero()
                {
                    Torneo = new Torneo() { Id = autPlanilleroDTO.TorneoId },
                    UsuarioOrganizador = new Usuario() { Id = autPlanilleroDTO.UsuarioIdOrganizador},
                    UsuarioPlanillero = new Usuario() { Id = autPlanilleroDTO.UsuarioIdPlanillero},
                };

                _db.Entry(autorizacionPlanillero.Torneo).State = EntityState.Unchanged;
                _db.Entry(autorizacionPlanillero.UsuarioOrganizador).State = EntityState.Unchanged;
                _db.Entry(autorizacionPlanillero.UsuarioPlanillero).State = EntityState.Unchanged;

                var registrado = await _db.AutorizacionesPlanilleros.AddAsync(autorizacionPlanillero);

                await _db.SaveChangesAsync();

                return registrado.Entity.Id;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task<bool> ValidarPlanillero(AutorizacionPlanilleroDTO autPlanilleroDTO)
        {

            bool validarPlanillero = await _db.AutorizacionesPlanilleros.Include(i => i.Torneo)
                                                                         .Include(i => i.UsuarioPlanillero)
                                                                         .AnyAsync(pl => pl.UsuarioPlanillero.Id == autPlanilleroDTO.UsuarioIdPlanillero
                                                                                          && pl.Torneo.Fecha == autPlanilleroDTO.FechaTorneo);
            return validarPlanillero;
        }


        public async Task<bool> QuitarAutorizacion(AutorizacionPlanilleroDTO autPlanilleroDTO)
        {
            if (autPlanilleroDTO == null) return false;
            AutorizacionPlanillero autoPlanillero = await _db.AutorizacionesPlanilleros.SingleOrDefaultAsync(s => s.UsuarioPlanillero.Id == autPlanilleroDTO.UsuarioIdPlanillero
                                                                                                                 && s.Torneo.Id == autPlanilleroDTO.TorneoId);

            _db.AutorizacionesPlanilleros.Remove(autoPlanillero);
            int borrado = await _db.SaveChangesAsync();

            return borrado > 0;
        }

        public async Task<bool> QuitarAutorizacionesFinTorneo(int torneoId)
        {
            if (torneoId < 0) return false;

            var autPlanilleros = await _db.AutorizacionesPlanilleros.Where(w => w.Torneo.Id == torneoId).ToListAsync();

            _db.AutorizacionesPlanilleros.RemoveRange(autPlanilleros);
            
            int borrados = await _db.SaveChangesAsync();

            return borrados > 0;
        }

        public async Task<List<AutorizacionPlanillero>> ObtenerAutorizacionesPlanillero(int planilleroId)
        {
            var listado = await _db.AutorizacionesPlanilleros.AsNoTracking()
                                                             .Include(i => i.Torneo)
                                                             .Include(i => i.UsuarioOrganizador)
                                                             .Include(i => i.UsuarioPlanillero)
                                                             .Where(w => w.UsuarioPlanillero.Id == planilleroId).ToListAsync();
            return listado;
        }

        public async Task<List<AutorizacionPlanillero>> ObtenerAutorizacionesPlanilleroOrganizador(int organizadorId)
        {
            var listado = await _db.AutorizacionesPlanilleros.AsNoTracking()
                                                             .Include(i => i.Torneo)
                                                             .Include(i => i.UsuarioOrganizador)
                                                             .Include(i => i.UsuarioPlanillero)
                                                             .Where(w => w.UsuarioOrganizador.Id == organizadorId).ToListAsync();
            return listado;
        }

        public async Task<AutorizacionPlanilleroDTO> ObtenerUsuarioParaAutorizar(string mail)
        {
            try
            {
                var usuario = await _db.Usuarios.SingleOrDefaultAsync(us => us.Mail == mail);

                if (usuario == null) throw new Exception("No se encuentra el usuario. W106");

                if (usuario.Rol != Util.Roles.PLANILLERO.ToString()) throw new Exception("El mail no corresponde a un usuario de tipo planillero. W202");

                var datosPlanillero = new AutorizacionPlanilleroDTO()
                {
                    UsuarioIdPlanillero = usuario.Id,
                    NombrePlanillero = usuario.Nombre,
                    EmailPlanillero = usuario.Mail
                };
                return datosPlanillero;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }



    }
}
