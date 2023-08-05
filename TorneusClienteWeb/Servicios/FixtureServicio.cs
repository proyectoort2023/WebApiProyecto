﻿using BDTorneus;
using DTOs_Compartidos.Models;
using FixtureNegocio;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using Negocio.DTOs;
using Utilidades;

namespace TorneusClienteWeb.Servicios
{
    public class FixtureServicio
    {

        List<PartidoDTO> Partidos = new List<PartidoDTO>();

        [Inject] private TorneoServicio _torneoServicio { get; set; }

        public FixtureServicio(TorneoServicio torneoServicio)
        {
            _torneoServicio = torneoServicio;
        }
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
                Partidos = new List<PartidoDTO>();

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

                int cantidadCanchas = _torneoServicio.ObtenerTorneoActual().CantidadCanchas;

                for (int i = 0; i < cantidadCanchas; i++)
                {
                    Partidos[i].EstadoPartido = Util.EstadoPartido.POR_COMENZAR.ToString(); //esto quiere decir que el siguiente partido si no tiene siguienteGuid, se tiene que iniciar en el siguiente partido pendiente
                }
              

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


        public string ObtenerTiempoJornada()
        {
            var torneoActual = _torneoServicio.ObtenerTorneoActual();

            int cantidadCanchas = torneoActual.CantidadCanchas;
            int cantidadSetsPorPartido = torneoActual.SetsMax;
            int puntajePorSet = torneoActual.PuntajeMax;
            int cantidadPartidos = Partidos.Count();

            double minutosPorSet = 20 * puntajePorSet / 25;
            double minutosPorPartido = minutosPorSet * cantidadSetsPorPartido;
            double jornada = minutosPorPartido * cantidadPartidos / cantidadCanchas; //minutos

            // Calcular las horas y minutos
            int horas = (int)(jornada / 60); 
            int minutosRestantes = (int)(jornada % 60);

            return $"{horas} hora/s y {minutosRestantes} minutos";
        }





    } 
}
