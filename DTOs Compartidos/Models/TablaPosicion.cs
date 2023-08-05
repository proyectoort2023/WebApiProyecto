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
            MejorTiempo = 0;
        }

        public void ActualizarTabla(int partidosJugados, int partidosGanados, int setsGanados, int totalPuntos, int mejorTiempo)
        {
            PartidosJugados += partidosJugados;
            PartidosGanados += partidosGanados;
            SetsGanados += setsGanados;
            TotalPuntos += totalPuntos;
            MejorTiempo += mejorTiempo;
        }

    }
}
