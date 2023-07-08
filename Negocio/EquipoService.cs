using BDTorneus;
using DTOs_Compartidos.Models;
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

        public async Task<bool> ModificarCapital(JugadorCapitan jugadorCapitan)
        {
            try
            {
                Jugador jugadorBuscado = await _db.Jugadores.FindAsync(jugadorCapitan.CapitanId);

                if (jugadorBuscado == null) throw new Exception("No se ha encontrado al jugador buscado. W022");

                jugadorBuscado.Capitan = jugadorCapitan.NuevoValor;

                int cantidadModificados = await _db.SaveChangesAsync();

                return cantidadModificados > 0;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
          
        }


    }
}
