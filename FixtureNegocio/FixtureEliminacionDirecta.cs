using BDTorneus;
using Microsoft.IdentityModel.Tokens;
using Negocio.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilidades;

namespace FixtureNegocio
{
    public class FixtureEliminacionDirecta
    {


        public List<PartidoDTO> Crear(List<EquipoDTO> equipos)
        {
            List<PartidoDTO> fixture = new();
            List<PartidoDTO> partidosPorRonda = new();

            int cantEquipos = equipos.Count;
            int cantPartidos = cantEquipos - 1;
            int cantRondas = CantidadRondasRecursivo(cantEquipos);
            int ajusteEqNoJueganPrimeraRonda = (int)Math.Pow(2, cantRondas) - cantEquipos;

            partidosPorRonda = equipos.Select(equipo => new PartidoDTO()
                                                    {
                                                        EquipoLocal = equipo,
                                                        Ronda = 0,
                                                        GuidPartido = Guid.NewGuid(),
                                                        EstadoPartido = Util.EstadoPartido.PENDIENTE.ToString()
                                                    }).ToList();

            fixture = CrearRecursivo(fixture, partidosPorRonda, ajusteEqNoJueganPrimeraRonda, cantRondas);

            return fixture;
        }


        public List<PartidoDTO> CrearComoSegundaFase(int cantidadEquipos)
        {
            List<PartidoDTO> fixture = new();
            List<PartidoDTO> partidosPorRonda = new();

            int cantEquipos = cantidadEquipos;
            int cantPartidos = cantEquipos - 1;
            int cantRondas = CantidadRondasRecursivo(cantEquipos);
            int ajusteEqNoJueganPrimeraRonda = (int)Math.Pow(2, cantRondas) - cantEquipos;

            for(int i = 0; i < cantEquipos; i++)
            {
                partidosPorRonda.Add(new PartidoDTO()
                {
                    EquipoLocal = null,
                    Ronda = 0,
                    GuidPartido = Guid.NewGuid(),
                    EstadoPartido= Util.EstadoPartido.PENDIENTE.ToString()
                });
            }

            fixture = CrearRecursivo(fixture, partidosPorRonda, ajusteEqNoJueganPrimeraRonda, cantRondas);

            return fixture;
        }



        private int CantidadRondasRecursivo(int cantEq, int total = 0, int jornadas = 0)
        {
            if (total >= cantEq) return jornadas;

            jornadas++;
            total = (int)Math.Pow(2, jornadas);

            return CantidadRondasRecursivo(cantEq, total, jornadas);
        }


        private List<PartidoDTO> CrearRecursivo(List<PartidoDTO> fixture, List<PartidoDTO> partidosPorRonda, int ajustePrimeraRonda, int cantidadRondas, int rondaActual = 1)
        {
            int cantPartidosPorRonda = partidosPorRonda.Count;

            if (cantPartidosPorRonda == 1) return fixture;

            //if (rondaActual > cantidadRondas) return fixture;   //por ronda o por partidosPorRonda.count == 1;


                List<PartidoDTO> auxFiltroFixturDescanso = new();
                List<PartidoDTO> auxFixturePorRonda = new();

                 int partidosJugadosRonda = cantPartidosPorRonda - ajustePrimeraRonda;

                //caso particular de ajuste de partidos
                if (rondaActual == 1)
                {
                    for (int i = 0; i < cantPartidosPorRonda - 1; i += 2)
                    {

                        PartidoDTO partido = new()
                        {
                            EquipoLocal = partidosPorRonda[i].EquipoLocal,
                            EquipoVisitante = partidosPorRonda[i+1].EquipoLocal,
                            GuidPartido = Guid.NewGuid(),
                            Ronda = rondaActual,
                            RondaDescanso = false,
                            EstadoPartido = Util.EstadoPartido.PENDIENTE.ToString()
                        };
                        auxFixturePorRonda.Add(partido);
                    }
               
                fixture.AddRange(auxFixturePorRonda);

                if (ajustePrimeraRonda > 0)
                {
                    auxFiltroFixturDescanso = partidosPorRonda.Skip(partidosJugadosRonda).Take(ajustePrimeraRonda).ToList();
                    auxFiltroFixturDescanso.ForEach(aux => aux.RondaDescanso = true);
                    auxFixturePorRonda.AddRange(auxFiltroFixturDescanso);
                }
                ajustePrimeraRonda = 0;
                rondaActual++;
                return CrearRecursivo(fixture,auxFixturePorRonda, ajustePrimeraRonda,cantidadRondas,rondaActual);
            }


            //caso comun
            for (int i = 0; i < cantPartidosPorRonda - 1; i += 2)
            {

                PartidoDTO partido = new()
                {
                    EquipoLocal = partidosPorRonda[i].RondaDescanso ? partidosPorRonda[i].EquipoLocal : null,
                    EquipoVisitante = partidosPorRonda[i+1].RondaDescanso ? partidosPorRonda[i+1].EquipoLocal : null,
                    GuidPartido = Guid.NewGuid(),
                    Ronda = rondaActual,
                    RondaDescanso = false,
                    EstadoPartido = Util.EstadoPartido.PENDIENTE.ToString()
                };

                partidosPorRonda[i].PartidoSigGanador = partido.GuidPartido;
                partidosPorRonda[i+1].PartidoSigGanador = partido.GuidPartido;

                auxFixturePorRonda.Add(partido);
            }

            fixture.AddRange(auxFixturePorRonda);
            rondaActual++;

            return CrearRecursivo(fixture, auxFixturePorRonda, ajustePrimeraRonda, cantidadRondas, rondaActual);
        }







    }
}
