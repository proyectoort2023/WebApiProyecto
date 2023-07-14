using BDTorneus;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Negocio.Validaciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio
{
    public class InscripcionService
    {

        private readonly TorneoContext _db;

        public InscripcionService(TorneoContext db)
        {
            _db = db;
        }


        public async Task<List<Inscripcion>> ObtenerInscripcionesSegunUsuario(int usuarioId)
        {
            try
            {
                if (usuarioId < 0) throw new Exception("El usuario no se encuentra para obtener las inscripciones");

                List<Inscripcion> inscripciones = await _db.Inscripciones.Include(i => i.Usuario)
                                                                          .Include(i => i.Usuario)
                                                                          .Include(i => i.Torneo)
                                                                          .Where(w => w.UsuarioId == usuarioId).ToListAsync();

                if (inscripciones == null) throw new Exception("No hay inscricpiones válidas");

                return inscripciones;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Inscripcion> AgregarNuevaInscripcion(Inscripcion inscripcion)
        {
            try
            {
                string mensajeError = "";

                ValidadorCreacionInscripcion validacion = new(_db);
                ValidationResult resultValidacion = validacion.Validate(inscripcion);
                if (!resultValidacion.IsValid)
                {
                    resultValidacion.Errors.ForEach(f => mensajeError += f.ErrorMessage);
                    throw new Exception(mensajeError);
                }


                inscripcion.Usuario = await _db.Usuarios.FindAsync(inscripcion.UsuarioId);
                inscripcion.Torneo = await _db.Torneos.FindAsync(inscripcion.TorneoId);
                inscripcion.Equipo = await _db.Equipos.FindAsync(inscripcion.EquipoId);

                _db.Entry(inscripcion.Usuario).State = EntityState.Unchanged;
                _db.Entry(inscripcion.Torneo).State = EntityState.Unchanged;
                _db.Entry(inscripcion.Equipo).State = EntityState.Unchanged;


                var inscrpcionNueva = await _db.Inscripciones.AddAsync(inscripcion);

                await _db.SaveChangesAsync();

                return inscrpcionNueva.Entity;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }







    }
}
