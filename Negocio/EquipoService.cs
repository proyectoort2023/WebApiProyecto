using BDTorneus;
using DTOs_Compartidos.Models;
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
    public class EquipoService
    {
        private readonly TorneoContext _db;

        public EquipoService(TorneoContext db)
        {
            _db = db;
        }


        public async Task<List<Equipo>> ObtenerEquiposDeAdministrador(int usuarioId)
        {
            try
            {
                List<Equipo> equipos = await _db.Equipos.AsNoTracking()
                                                        .Include(i => i.Jugadores)
                                                        .Where(w => w.UsuarioId == usuarioId).ToListAsync();

                return equipos;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task<int> CrearEquipoNuevo(Equipo equipoNuevo)
        {
            try
            {
                if (equipoNuevo == null) throw new Exception("El equipo no tiene datos para guardar . W50");
                string mensajeError = "";

                ValidadorEquipo validacion = new(_db);
                ValidationResult resultado = validacion.Validate(equipoNuevo);
                if (!resultado.IsValid)
                {
                    resultado.Errors.ForEach(f => mensajeError += f.ErrorMessage);
                    throw new Exception(mensajeError);
                }
               
                foreach (var jugador in equipoNuevo.Jugadores)
                {
                    _db.Entry(jugador).State = EntityState.Unchanged;
                }

                var equipoRegistrado = await _db.Equipos.AddAsync(equipoNuevo);
                await _db.SaveChangesAsync();

                return equipoRegistrado.Entity.Id;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }




    }
}
