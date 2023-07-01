using BDTorneus;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;
using Negocio.DTOs;
using Negocio.Validaciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio
{
    public class TorneoService
    {
        private readonly TorneoContext _db;
        private readonly IConfiguration _configuration;

        public TorneoService(TorneoContext db, IConfiguration configuration)
        {
            _db = db;
            _configuration = configuration;
        }


        public async Task<Torneo> CrearTorneo(Torneo torneo)
        {
            try
            {
                if (torneo == null)  throw new Exception("El torneo no tiene datos para salvar"); 
                string mensajeError = "";

                ValidarTorneoDuplicado validacionDuplicado = new(_db);
                ValidationResult resultDuplicado = validacionDuplicado.Validate(torneo);
                if (!resultDuplicado.IsValid)
                {
                    resultDuplicado.Errors.ForEach(f => mensajeError += f.ErrorMessage);
                    throw new Exception(mensajeError);
                }

                ValidadorTorneo validacion = new(_db);
                ValidationResult result = validacion.Validate(torneo);
                if (!result.IsValid)
                {
                    result.Errors.ForEach(f => mensajeError += f.ErrorMessage);
                    throw new Exception(mensajeError);
                }
                var usuarioBuscado = await _db.Usuarios.FindAsync(torneo.Usuario.Id);
                if (usuarioBuscado == null) throw new Exception("El usuario organizador no existe");

                torneo.Usuario = usuarioBuscado;
                _db.Entry(torneo.Usuario).State = EntityState.Unchanged;

                var torneoNuevo = await _db.Torneos.AddAsync(torneo);
                await _db.SaveChangesAsync();

                return torneoNuevo.Entity;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task<bool> SuspenderTorneo(int torneoId)
        {
            try
            {
                Torneo torneo = await _db.Torneos.FindAsync(torneoId);

                if (torneo == null) throw new Exception("El torneo no tiene datos para salvar");

                if (torneo.Fecha.Date.AddDays(-1) < DateTime.Today.Date) throw new Exception("El torneo no se puede suspender porque los equipos están jugando o el torneo ha finalizado");

                torneo.HabilitacionInscripcion = false;
                torneo.Suspendido = true;

                int updateRealizado = await _db.SaveChangesAsync();

                // Hay que resolver el tema del reembolso de aquellos que pagaron por pasarela de pago online
                bool resultado = updateRealizado > 0;
                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> EliminarTorneo(int torneoId)
        {
            try
            {
                if (torneoId < 1) throw new Exception("El torneo que quiere eliminar no existe");
                
                Torneo torneoAEliminar = await _db.Torneos.Include(i => i.Inscripciones)
                                                           .SingleOrDefaultAsync(t => t.Id == torneoId);

                if (torneoAEliminar == null) throw new Exception("El torneo que quiere eliminar no existe");
                if (torneoAEliminar.Inscripciones.Count > 0) throw new Exception("No puede eliminar porque hay equipos inscriptos");
                if (torneoAEliminar.HabilitacionInscripcion) throw new Exception("No puede eliminar si tiene inscripciones abiertas");
                if (torneoAEliminar.Suspendido) throw new Exception("No puede eliminar el torneo porque está suspendido");
                if (torneoAEliminar.Fecha <= DateTime.Today.Date) throw new Exception("No puede eliminar el torneo ya que está en progreso o finalizado");

                _db.Torneos.Remove(torneoAEliminar);
                int eliminacionRealizada = await _db.SaveChangesAsync();

                // Hay que resolver el tema del reembolso de aquellos que pagaron por pasarela de pago online
                return eliminacionRealizada > 0;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task<bool> ModificarTorneo(Torneo torneo)
        {
            try
            {
                if (torneo == null) throw new Exception("El torneo no tiene datos para editar");
                string mensajeError = "";

                ValidadorActualizacionTorneo validacion = new(_db);
                ValidationResult result = validacion.Validate(torneo);
                if (!result.IsValid)
                {
                    result.Errors.ForEach(f => mensajeError += f.ErrorMessage);
                    throw new Exception(mensajeError);
                }
                Torneo torneoBuscado = await _db.Torneos.FindAsync(torneo.Id);
                if (torneoBuscado == null) throw new Exception("El torneo no existe");


                torneoBuscado.Fecha = torneo.Fecha;
                torneoBuscado.HoraComienzo = torneo.HoraComienzo;
                torneoBuscado.NombreContacto = torneo.NombreContacto;
                torneoBuscado.TelContacto = torneo.TelContacto;
                torneoBuscado.Precio = torneo.Precio;
                torneoBuscado.Lugar = torneo.Lugar;
                torneoBuscado.TipoPrecio = torneo.TipoPrecio;
                torneoBuscado.SetsMax = torneo.SetsMax;
                torneoBuscado.PuntajeMax = torneo.PuntajeMax;
                torneoBuscado.PuntajeMaxUltimoSet = torneo.PuntajeMaxUltimoSet;
                torneoBuscado.ConfiguracionEquipos = torneo.ConfiguracionEquipos;
                torneoBuscado.Otros = torneo.Otros;
                torneoBuscado.MaxEquiposInscriptos = torneo.MaxEquiposInscriptos;
                torneoBuscado.MaxJugadoresPorEquipo = torneo.MaxJugadoresPorEquipo;
                torneoBuscado.CantidadCanchas = torneo.CantidadCanchas;
                torneoBuscado.Indoor = torneo.Indoor;


                var torneoNuevo = _db.Update(torneoBuscado);
                var registrosActualizados = await _db.SaveChangesAsync();

                return registrosActualizados > 0;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task<List<Torneo>> MisTorneosOrganizador(int idOrganizador)
        {
            var listaTorneos = await _db.Torneos.Where(w => w.Usuario.Id == idOrganizador)
                                            .OrderByDescending(o => o.Fecha)
                                            .ToListAsync();

            return listaTorneos;
        }

        public async Task<List<Torneo>> ObtenerTorneosVigentes()
        {
            var listaTorneos = await _db.Torneos.OrderByDescending(o => o.Fecha)
                                                .ToListAsync();

            return listaTorneos;
        }


        public async Task<List<TorneoInscripcionAbiertaDTO>> ListadoInscripcionesAbierta()
        {
            var listaTorneosHabilitaciones = await _db.Torneos.Where(t => t.Fecha > DateAndTime.Today && t.HabilitacionInscripcion == true)
                                                .Select(s => new TorneoInscripcionAbiertaDTO()
                                                {
                                                     IdTorneo = s.Id,
                                                     FechaComienzo = s.Fecha
                                                })
                                                .ToListAsync();             

            return listaTorneosHabilitaciones;
        }


        public async Task<bool> CerrarInscripciones(TorneoInscripcionAbiertaDTO torneoInscAb)
        {
          var torneo = await _db.Torneos.FindAsync(torneoInscAb.IdTorneo);

            if (torneo == null) return false;

            torneo.HabilitacionInscripcion = false;

            int cantidadRegistros = await _db.SaveChangesAsync();

            return cantidadRegistros > 1;

        }

        public async Task<(bool, DateTime?)> AbrirInscripciones(int idTorneo)
        {
            var torneo = await _db.Torneos.FindAsync(idTorneo);

            if (torneo == null) return (false,null);

            torneo.HabilitacionInscripcion = true;

            int cantidadRegistros = await _db.SaveChangesAsync();

            bool resultado = cantidadRegistros > 1;

            return (resultado, torneo.Fecha);

        }


    }
}
