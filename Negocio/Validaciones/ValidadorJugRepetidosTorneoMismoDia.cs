using BDTorneus;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.Validaciones
{
    public  class ValidadorJugRepetidosTorneoMismoDia
    {
        private  readonly TorneoContext _db;

        public ValidadorJugRepetidosTorneoMismoDia(TorneoContext db)
        {
            _db = db;
        }

        public async Task<bool> BuscarRepetidos(Inscripcion inscripcion)
        {
            try
            {
                Torneo torneo = await _db.Torneos.SingleOrDefaultAsync(tor => tor.Id == inscripcion.TorneoId);

                List<Equipo> equipos = _db.Inscripciones.Include(ins => ins.Equipo.Jugadores)
                                                              .Where(ins => ins.Torneo.Fecha == torneo.Fecha)
                                                              .Select(ins => ins.Equipo).ToList();

                Equipo equipoActual = await _db.Equipos.Include(i => i.Jugadores).SingleOrDefaultAsync(eq => eq.Id == inscripcion.EquipoId);

                bool algunJugadorRepetido = equipos.SelectMany(equipo => equipo.Jugadores)
                                                   .Any(jugador => equipoActual.Jugadores.Contains(jugador));

                return algunJugadorRepetido;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
              
        }
    }
}
