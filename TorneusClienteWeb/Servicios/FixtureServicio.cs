using BDTorneus;
using DTOs_Compartidos.Models;
using FixtureNegocio;
using Microsoft.Extensions.Options;
using Negocio.DTOs;

namespace TorneusClienteWeb.Servicios
{
    public class FixtureServicio
    {

        List<PartidoDTO> Partidos;

        public List<PartidoDTO> ObtenerPartidos()
        {
            return Partidos;
        }

        public async Task<bool> CrearFixtureGrupoEliminacionDirecta(List<SelectEquipo> selectEquipos, int cantidadGrupos)
        {
            try
            {
                if (VerificarMinimoPorGrupo(selectEquipos)) throw new Exception("Cada grupo debe tener un minimo de 3 equipos");


                FixtureGrupos fixture = new();
                FixtureEliminacionDirecta fixtureElimDirecta = new();

                List<GrupoEquipos> grupoequipos = selectEquipos.GroupBy(g => g.Grupo)
                                                         .Select(s => new GrupoEquipos()
                                                         {
                                                             Grupo = s.Key,
                                                             Equipos = s.Select(eq => eq.Equipo).ToList()
                                                         }).ToList();

                Partidos = fixture.Crear(grupoequipos);

                int cantidadEquiposUltimaFase = CantidadEquiposUltimaFase(cantidadGrupos);


                List<PartidoDTO> partidosUltimaFase = fixtureElimDirecta.CrearComoSegundaFase(cantidadEquiposUltimaFase);

                Partidos.AddRange(partidosUltimaFase);

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
           
        }

        private bool VerificarMinimoPorGrupo(List<SelectEquipo> selectEquipos)
        {
            Dictionary<string, int> equiposPorGrupo = new Dictionary<string, int>();

            foreach (var equipo in selectEquipos)
            {
                if (equiposPorGrupo.ContainsKey(equipo.Grupo))
                {
                    equiposPorGrupo[equipo.Grupo]++;
                }
                else
                {
                    equiposPorGrupo.Add(equipo.Grupo, 1);
                }
            }

            // Verificar si algún grupo tiene menos de 3 equipos
            bool hayGrupoConMenosDe3Equipos = equiposPorGrupo.Any(grupo => grupo.Value < 3);

            return hayGrupoConMenosDe3Equipos;

        }

        private int CantidadEquiposUltimaFase(int cantidad)
        {
            double valor = 1;
            int incrementador = 0;

            while (valor < cantidad)
            {
                incrementador++;
                valor = Math.Pow(2, incrementador);
            }

            return (int)valor;
        }






    } 
}
