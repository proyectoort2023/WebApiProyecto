using BDTorneus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.DTOs
{
    public class PartidoDTO
    {
        public int Id { get; set; }
        public int EquipoLocalId { get; set; }
        public int EquipoVisitanteId { get; set; }
        public DateTime Fecha { get; set; }
        public int MarcadorLocal { get; set; }
        public int MarcadorVisitante { get; set; }
        public int SetGanadosLocal { get; set; }
        public int SetGanadosVisitante { get; set; }
        public int SetActual { get; set; }
        public int PuntajeLocal { get; set; }
        public int PuntajeVisitante { get; set; }
        public int TorneoId { get; set; }
        public TorneoDTO Torneo { get; set; }
        public string NombreCancha { get; set; }
        public DateTime Inicio { get; set; }
        public DateTime Fin { get; set; }
        public string HistorialSet { get; set; }
        public int Ronda { get; set; }
        public Guid GuidId { get; set; }
        public Guid PartidoSiguienteGuidId { get; set; }
    }
}
