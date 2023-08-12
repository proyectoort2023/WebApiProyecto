﻿using BDTorneus;
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

            foreach (var grupoEquipo in grupoEquipos)
            {
                List<PartidoDTO> partidosGrupo = todosContraTodosGrupo.Crear(grupoEquipo.Equipos, grupoEquipo.Grupo);
                fixtureAux.AddRange(partidosGrupo);
            }

            if (grupoEquipos.Count > 1)
            {
                fixture = ReordenamientoPosicionesPartidosPorGrupo(fixtureAux);
            }
            else
            {
                fixture = ReordenamientoPosicionesPartidosPorEQuipo(fixtureAux);
            }
            fixture.Last().DisparadorSiguienteFase = true; //cuando llega al ultimo partido del grupo dispara una alerta de siguiente fase en el torneo

            //falta etapa eleccion de mejores ganadores y mejores jugadores de cada grupo y de eliminacion directa


            return fixture;

        }


        public List<PartidoDTO> ReordenamientoPosicionesPartidosPorGrupo(List<PartidoDTO> partidos)
        {
            List<PartidoDTO> partidosReordenados = PosicionPartidosAleatorios(new List<PartidoDTO>(), partidos, partidos.Count);

            return partidosReordenados;
        }

        public List<PartidoDTO> ReordenamientoPosicionesPartidosPorEQuipo(List<PartidoDTO> partidos)
        {
            List<PartidoDTO> partidosReordenados = PosicionPartidosAleatoriosPorEquipo(new List<PartidoDTO>(), partidos, partidos.Count);

            return partidosReordenados;
        }

        private List<PartidoDTO> PosicionPartidosAleatorios(List<PartidoDTO> partidos, List<PartidoDTO> partidosAux, int cantidad, bool faseInicial = true)
        {
            int repetidos = partidosAux.Select(p => p.Grupo).Distinct().Count();
            //Random random = new Random();

            int index = 0;

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
                cantidad -= 1;
                return PosicionPartidosAleatorios(partidos,partidosAux, cantidad, faseInicial);
            }
            string ultimoPartidoGrupo = partidos.Last().Grupo;

            while (ultimoPartidoGrupo == partidosAux[index].Grupo)
            {
                index++;
            }

            partidos.Add(partidosAux[index]);
            partidosAux.RemoveAt(index);
            cantidad -= 1;

            return PosicionPartidosAleatorios(partidos, partidosAux, cantidad, faseInicial);
        }


        private List<PartidoDTO> PosicionPartidosAleatoriosPorEquipo(List<PartidoDTO> partidos, List<PartidoDTO> partidosAux, int cantidad, bool faseInicial = true)
        {
            Random random = new Random();

            int index = random.Next(0, cantidad - 1);

            if (cantidad < 3)
            {
                partidos.AddRange(partidosAux);
                return partidos;
            }

            if (faseInicial)
            {
                partidos.Add(partidosAux.First());
                partidosAux.RemoveAt(0);
                faseInicial = false;
                cantidad -= 1;
                return PosicionPartidosAleatoriosPorEquipo(partidos, partidosAux, cantidad, faseInicial);
            }
            int ultimoPartidoLocal = partidos.Last().EquipoLocal.Id;
            int ultimoPartidoVisistante = partidos.Last().EquipoVisitante.Id;

            while (ultimoPartidoLocal == partidosAux[index].EquipoLocal.Id && ultimoPartidoVisistante == partidosAux[index].EquipoVisitante.Id)
            {
                index = random.Next(0, cantidad - 1);
            }

            partidos.Add(partidosAux[index]);
            partidosAux.RemoveAt(index);
            cantidad -= 1;

            return PosicionPartidosAleatoriosPorEquipo(partidos, partidosAux, cantidad, faseInicial);
        }



    }
}
