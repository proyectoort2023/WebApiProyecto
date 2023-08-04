using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BDTorneus
{
    [DataContract(IsReference = true)]
    public class Partido
    {
        public int Id { get; set; }
        public int EquipoLocalId { get; set; }
        public Equipo EquipoLocal { get; set; }
        public int EquipoVisitanteId { get; set; }
        public Equipo EquipoVisitante { get; set; }
        public DateTime Fecha { get; set; }
        public int MarcadorLocal { get; set; }
        public int MarcadorVisitante { get; set; }
        public int SetGanadosLocal { get; set; }
        public int SetGanadosVisitante { get; set; }
        public int SetActual { get; set; }
        public int PuntajeLocal { get; set; }
        public int PuntajeVisitante { get; set; }
        public int TorneoId { get; set; }
        public Torneo Torneo { get; set; }
        public string NombreCancha { get; set; }
        public DateTime Inicio { get; set; }
        public DateTime Fin { get; set; }
        public string HistorialSet { get; set; }
        public int Ronda { get; set; }


        public bool RondaDescanso { get; set; }
        public string EstadoPartido { get; set; }
        public Guid GuidPartido { get; set; }
        public Guid PartidoSigGanador { get; set; }
        public Guid PartidoSigPerdedor { get; set; }

        public string SeleccionEquipoDelGrupo { get; set; }
        public string Grupo { get; set; }
        public bool DisparadorSiguienteFase { get; set; }


    }
}
