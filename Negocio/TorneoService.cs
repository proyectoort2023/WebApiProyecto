using BDTorneus;
using BDTorneus.Migrations;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;
using Negocio.DTOs;
using Negocio.Validaciones;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
                torneo.Otros = string.IsNullOrEmpty(torneo.Otros) ? "" : torneo.Otros;
                torneo.UbicacionGPS = string.IsNullOrEmpty(torneo.UbicacionGPS) ? "" : torneo.UbicacionGPS;
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
                if (torneo.HabilitacionInscripcion) throw new Exception("El torneo no se puede suspender porque hay inscripciones abiertas");

                torneo.Suspendido = true;
                torneo.Cerrrado = true;

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


        public async Task<Torneo> ModificarTorneo(Torneo torneo)
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

                if (registrosActualizados == 0) throw new Exception("No se ha podido actualizar . Revise");

                return torneoNuevo.Entity;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task<List<Torneo>> MisTorneosOrganizador(int idOrganizador)
        {
            var listaTorneos = await _db.Torneos.Where(w => w.Usuario.Id == idOrganizador)
                                            .AsNoTracking()
                                            .OrderByDescending(o => o.Fecha)
                                            .ToListAsync();

            return listaTorneos;
        }

        public async Task<List<Torneo>> ObtenerTorneosVigentes()
        {
            var listaTorneos = await _db.Torneos.Include(inc => inc.Usuario)
                                                .AsNoTracking()
                                                .Where(w => w.Cerrrado == false)
                                                .OrderByDescending(o => o.Fecha.Date)
                                                .ToListAsync();

            listaTorneos.ForEach(t => {
                t.Usuario.Pass = "";
                t.Usuario.AccessTokenMercadopago = "";
                t.Usuario.AccessTokenRefreshMercadopago ="";
            });

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


        public async Task<bool> CerrarInscripciones(int idTorneo)
        {
          var torneo = await _db.Torneos.FindAsync(idTorneo);

            if (torneo == null) return false;
            if (torneo.Fecha.Date <= DateTime.Today.Date) throw new Exception("No se puede cerrar inscripciones en el dia del evento o posterior");
            if (torneo.Cerrrado) throw new Exception("No se puede cerrar inscripciones ya que el evento está cerrado");
            if (torneo.Suspendido) throw new Exception("No se puede cerrar inscripciones ya que el evento está suspendido");

            torneo.HabilitacionInscripcion = false;

            int cantidadRegistros = await _db.SaveChangesAsync();

            bool resultado = cantidadRegistros > 0;

            return resultado;

        }

        public async Task<bool> AbrirInscripciones(int idTorneo)
        {
            try
            {
                var torneo = await _db.Torneos.FindAsync(idTorneo);

                if (torneo == null) return (false);
                if (torneo.Fecha.Date <= DateTime.Today.Date) throw new Exception("No se puede abrir inscripciones en el dia del evento o posterior");
                if (torneo.Cerrrado) throw new Exception("No se puede abrir inscripciones ya que el evento está cerrado");
                if (torneo.Suspendido) throw new Exception("No se puede abrir inscripciones ya que el evento está suspendido");

                torneo.HabilitacionInscripcion = true;

                int cantidadRegistros = await _db.SaveChangesAsync();

                bool resultado = cantidadRegistros > 0;

                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<Inscripcion>> ObtenerInscripcionesTorneo(int torneoId)
        {
            var listainscripciones = await _db.Inscripciones.Include(inc => inc.Equipo)
                                                            .Include(inc => inc.Torneo)
                                                            .Where(w => w.Torneo.Id == torneoId)
                                                            .ToListAsync();
            return listainscripciones;
        }



        public async Task<bool> TorneoEstaCerrado(int torneoId)
        {
            var torneo = await _db.Torneos.SingleOrDefaultAsync(w => w.Id == torneoId);

            return torneo.Cerrrado;
        }





    }
}
