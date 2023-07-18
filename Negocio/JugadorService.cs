using BDTorneus;
using DTOs_Compartidos.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio
{
    public class JugadorService
    {
        private readonly TorneoContext _db;
        public JugadorService(TorneoContext db)
        {
            _db = db;
        }


        public async Task<List<Jugador>> ObtenerTodosJugadores()
        {
            try
            {
                List<Jugador> jugadores = await _db.Jugadores.AsNoTracking().ToListAsync();

                return jugadores;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<int> RegistrarJugador(Jugador jugador)
        {
            try
            {
                bool jugadorDuplicado = await JugadorDuplicado(jugador.Cedula);

                if (jugadorDuplicado) throw new Exception("El jugador está duplicado");

                jugador.NombreCompleto.ToUpper().Trim();

                var nuevoJugador = await _db.Jugadores.AddAsync(jugador);
                await _db.SaveChangesAsync();
                return nuevoJugador.Entity.Id;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> JugadorDuplicado(string cedula)
        {
            bool duplicado = await _db.Jugadores.AnyAsync(a => a.Cedula == cedula);
            return duplicado;
        }






    }
}
