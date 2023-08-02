using BDTorneus;
using DTOs_Compartidos.Models;
using Negocio.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FixtureNegocio
{
    public class FixtureGrupos
    {

        public List<PartidoDTO> Crear(List<GrupoEquipos> grupoEquipos)
        {
            List<PartidoDTO> fixture = new List<PartidoDTO>();
            List<PartidoDTO> fixtureAux = new List<PartidoDTO>();
            FixtureTodosContraTodos todosContraTodosGrupo = new();

            foreach (var grupo in grupoEquipos)
            {
                List<PartidoDTO> partidosGrupo = todosContraTodosGrupo.Crear(grupo.Equipos);
                fixtureAux.AddRange(partidosGrupo);
            }

            List<PartidoDTO> partidosGrupoReordenadoPosicion = ReordenamientoPosicionesPartidos(fixtureAux);

            //falta etapa eleccion de mejores ganadores y mejores jugadores de cada grupo y de eliminacion directa


            return fixture;

        }


        public List<PartidoDTO> ReordenamientoPosicionesPartidos(List<PartidoDTO> partidos)
        {
            List<PartidoDTO> partidosReordenados = PosicionPartidosAleatorios(new List<PartidoDTO>(), partidos, partidos.Count);

            return partidosReordenados;
        }

        private List<PartidoDTO> PosicionPartidosAleatorios(List<PartidoDTO> partidos, List<PartidoDTO> partidosAux, int cantidad, bool faseInicial = true)
        {
            int repetidos = partidosAux.Select(p => p.Grupo).Distinct().Count();
            Random random = new Random();

            int index = random.Next(0, partidosAux.Count - 1);

            if (cantidad == 1 || repetidos == 1)
            {
                partidos.AddRange(partidosAux);
                return partidos;
            }

            if (faseInicial)
            {
                partidos.Add(partidosAux.First());
                partidosAux.RemoveAt(0);
                faseInicial = false;
                return PosicionPartidosAleatorios(partidos,partidosAux, cantidad--, faseInicial);
            }
            string ultimoPartidoGrupo = partidos.Last().Grupo;

            while (ultimoPartidoGrupo == partidosAux[index].Grupo)
            {
                index = random.Next(0,cantidad - 1);
            }

            partidos.Add(partidosAux[index]);
            partidosAux.RemoveAt(index);

            return PosicionPartidosAleatorios(partidos, partidosAux, cantidad--, faseInicial);
        }



    }
}
