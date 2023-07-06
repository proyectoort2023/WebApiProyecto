using BDTorneus;
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

                List<Inscripcion> inscripciones = await _db.Inscripciones.Where(w => w.UsuarioId == usuarioId).ToListAsync();

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
