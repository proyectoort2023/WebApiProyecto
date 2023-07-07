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
                List<Equipo> equipos = await _db.Equipos.Include(i => i.Jugadores)
                                                        .Where(w => w.UsuarioId == usuarioId).ToListAsync();

                return equipos;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<Jugador>> ObtenerTodosJugadores()
        {
            try
            {
                List<Jugador> jugadores = await _db.Jugadores.ToListAsync();

                return jugadores;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


    }
}
