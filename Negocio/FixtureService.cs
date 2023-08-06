using BDTorneus;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio
{
    public class FixtureService
    {

        private readonly TorneoContext _db;

        public FixtureService(TorneoContext db)
        {
            _db = db;
        }


        public async Task<List<Partido>> CrearFixture(int torneoId, List<Partido> partidos)
        {
            try
            {
                Torneo torneo = await _db.Torneos.Include(i => i.Inscripciones)
                                                 .ThenInclude(t => t.Equipo)
                                                 .SingleOrDefaultAsync(s => s.Id == torneoId);

                foreach (var partido in partidos)
                {
                    if (partido.EquipoLocal != null)
                    {
                        int idLocal = partido.EquipoLocal.Id;
                        partido.EquipoLocal = torneo.Inscripciones.SingleOrDefault(w => w.Equipo.Id == idLocal).Equipo;
                        _db.Entry(partido.EquipoLocal).State = EntityState.Unchanged;
                    }
                    else
                    {
                        partido.EquipoLocal = null;
                        partido.EquipoLocalId = null;
                    }

                    if (partido.EquipoVisitante != null)
                    {
                        int idVisitante = partido.EquipoVisitante.Id;
                        partido.EquipoVisitante = torneo.Inscripciones.SingleOrDefault(w => w.Equipo.Id == idVisitante).Equipo;
                        _db.Entry(partido.EquipoVisitante).State = EntityState.Unchanged;
                    }
                    else
                    {
                        partido.EquipoVisitante = null;
                        partido.EquipoVisitanteId = null;
                    }

                    partido.Torneo = torneo;
                }

                _db.Partidos.AddRange(partidos);
                await _db.SaveChangesAsync();

                return partidos;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<Partido>> ObtenerPartidosTorneo(int torneoId)
        {
            try
            {
                var partidos = await _db.Partidos.Include(i => i.EquipoLocal)
                                                 .Include(i => i.EquipoVisitante)
                                                  .Where(w => w.Torneo.Id == torneoId).ToListAsync();

                return partidos;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }






    }
}
