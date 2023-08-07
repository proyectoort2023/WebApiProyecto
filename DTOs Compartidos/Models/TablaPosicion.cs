using BDTorneus;
using Negocio.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs_Compartidos.Models
{
    public class TablaPosicion
    {
        public EquipoDTO Equipo { get; set; }
        public int PartidosJugados { get; set; }
        public int PartidosGanados { get; set; }
        public int SetsGanados { get; set; }
        public int TotalPuntos { get; set; }
        public int TantosTotales { get; set; }
        public int MejorTiempo { get; set; } //en minutos
        public string Grupo { get; set; }


        public TablaPosicion() { }
            public TablaPosicion(EquipoDTO equipo, string grupo)
        {
            Equipo = equipo;
            Grupo = grupo;
            PartidosJugados = 0;
            PartidosGanados = 0;
            SetsGanados = 0;
            TotalPuntos = 0;
            TantosTotales= 0;
            MejorTiempo = 0;
        }

        public void ActualizarTabla(int setsGanados, int puntosObtenidos, DateTime inicio, DateTime fin, int tantosRealizados)
        {
            PartidosJugados += 1;
            PartidosGanados += puntosObtenidos > 1 ? 1 : 0;
            SetsGanados += setsGanados;
            TotalPuntos += puntosObtenidos;
            TantosTotales += tantosRealizados;

            TimeSpan diferencia = fin - inicio;
            double minutos = diferencia.TotalMinutes;

            MejorTiempo += (int)minutos; //en minutos
        }

        //public void AgregarNuevoEquipo( int setsGanados, int puntosObtenidos, DateTime inicio, DateTime fin)
        //{
        //    PartidosJugados += 1;
        //    PartidosGanados += puntosObtenidos > 1 ? 1 : 0;
        //    SetsGanados += setsGanados;
        //    TotalPuntos += puntosObtenidos;

        //    TimeSpan diferencia = fin - inicio;
        //    double minutos = diferencia.TotalMinutes;

        //    MejorTiempo += (int)minutos; //en minutos
        //}

    }
}
