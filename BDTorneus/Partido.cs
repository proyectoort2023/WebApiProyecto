using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
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
        [ForeignKey("EquipoLocal")]
        public int? EquipoLocalId { get; set; }
        public virtual Equipo? EquipoLocal { get; set; } = new();
        [ForeignKey("EquipoVisitante")]
        public int? EquipoVisitanteId { get; set; }
        public virtual Equipo? EquipoVisitante { get; set; } = new();
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
        public string? NombreCancha { get; set; } = string.Empty;
        public DateTime Inicio { get; set; }
        public DateTime Fin { get; set; }
        public string? HistorialSet { get; set; } = string.Empty;
        public int Ronda { get; set; }


        public bool RondaDescanso { get; set; }
        public string? EstadoPartido { get; set; } = string.Empty;
        public Guid GuidPartido { get; set; }
        public Guid PartidoSigGanador { get; set; }
        public Guid PartidoSigPerdedor { get; set; }

        public string? SeleccionEquipoDelGrupo { get; set; } = string.Empty;
        public string? Grupo { get; set; } = string.Empty;
        public bool DisparadorSiguienteFase { get; set; }


    }
}
