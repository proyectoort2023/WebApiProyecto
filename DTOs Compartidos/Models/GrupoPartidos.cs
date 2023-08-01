using Negocio.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs_Compartidos.Models
{
    public class GrupoPartidos
    {
        public string LetraEquipo { get; set; }
        public List<PartidoDTO> Partidos { get; set; }
        public List<TablaPosicion> TablaPosiciones { get; set; }

        public GrupoPartidos(string letraEquipo, List<PartidoDTO> partidos)
        {
            CrearTablaPosiciones(partidos);
            //iterar por grupo y crear un todos contra todos
            //luego emparejar a b c por partido
        }

        private void CrearTablaPosiciones(List<PartidoDTO> partidos)
        {
            List<EquipoDTO> equipos = ObtenerEquipos(partidos);

            foreach(var equipo in equipos)
            {
                TablaPosiciones.Add( new TablaPosicion(equipo));
            }
        }

        private List<EquipoDTO> ObtenerEquipos(List<PartidoDTO> partidos)
        {
            List<EquipoDTO> equipos = new List<EquipoDTO>();

           for (int i = 0; i < partidos.Count -1; i++)
            {
                EquipoDTO equipoLocal = partidos[i].EquipoLocal;
                EquipoDTO equipoVisitante = partidos[i].EquipoVisitante;

                equipos.Add(equipoLocal);
                equipos.Add(equipoVisitante);
            }

            List<EquipoDTO> equiposSinDuplicaciones = equipos.DistinctBy(eq => eq.Id).ToList();

            return equiposSinDuplicaciones;
        }

    }
}
