using Negocio.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs_Compartidos.Models
{
    public class GrupoTablaPosicionesView
    {
        public string Grupo { get; set; }
        public List<TablaPosicion> TablaPosiciones { get; set; }

        public void ArmarTablaPosiciones(List<PartidoDTO> partidos)
        {

        }

        public void ActualizarPosicionEquipos(int partidosJugados, int partidosGanados, int setsGanados, int totalPuntos, int mejorTiempo, int equipoId)
        {
            int indice = BuscarIndice(equipoId);
            var tabla = TablaPosiciones[indice];

            tabla.PartidosJugados += partidosJugados;
            tabla.PartidosGanados += partidosGanados;
            tabla.SetsGanados += setsGanados;
            tabla.TotalPuntos += totalPuntos;
            tabla.MejorTiempo += mejorTiempo;

            ActualizarTablaGeneral();
        }

        public void ActualizarTablaGeneral()
        {
            var tablaOrdenada = TablaPosiciones.OrderByDescending(x => x.PartidosGanados)
                                                .ThenByDescending(x => x.SetsGanados)
                                                .ThenByDescending(x => x.TotalPuntos)
                                                .ThenByDescending(x => x.MejorTiempo)
                                                .ThenByDescending(x => x.PartidosJugados);
        }

        public List<EquipoDTO> ListaEquipoMejoresPorGrupo()
        {
            return null;
        }

        private int BuscarIndice(int equipoId)
        {
             return TablaPosiciones.FindIndex(f => f.Equipo.Id == equipoId);
        }
    }
}
