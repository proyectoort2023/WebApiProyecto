using Negocio.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilidades;
using static Utilidades.Util;

namespace FixtureNegocio
{
    public class FixtureTodosContraTodos
    {

       public List<PartidoDTO> Crear(List<EquipoDTO> equipos, string grupo)
        {
            List<PartidoDTO> partidos = new List<PartidoDTO>();

            partidos = CrearRecursivo(partidos, equipos, grupo);

            return partidos;
        }

        private List<PartidoDTO> CrearRecursivo(List<PartidoDTO> partidos, List<EquipoDTO> equipos, string grupo)
        {
            if (equipos.Count == 2)
            {
                PartidoDTO partido = new PartidoDTO()
                {
                    EquipoLocal = equipos[0],
                    EquipoVisitante = equipos[1],
                    GuidPartido = new Guid(),
                    Grupo = grupo,
                    EstadoPartido = Util.EstadoPartido.PENDIENTE.ToString()
                };
                partidos.Add(partido);
                return partidos;
            }

            EquipoDTO equipoAactual = equipos[0];

            for(int i=1; i<equipos.Count; i++)
            {
                PartidoDTO partido = new PartidoDTO()
                {
                    EquipoLocal = equipoAactual,
                    EquipoVisitante = equipos[i],
                    GuidPartido = new Guid(),
                    Grupo = grupo,
                    EstadoPartido = Util.EstadoPartido.PENDIENTE.ToString()
                };
                partidos.Add(partido);
            }

            equipos.RemoveAt(0);

            return CrearRecursivo(partidos, equipos, grupo);
        }







    }
}
