using Negocio.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs_Compartidos.DTOs
{
    public class NotificacionDTO
    {
        public int Id { get; set; }
        public int TorneoId { get; set; }
        public int EquipoId { get; set; }
        public string Mensaje { get; set; }
        public bool Leido { get; set; }
        public bool General { get; set; }
        public DateTime FechaHora { get; set; }
        public TorneoDTO Torneo { get; set; }
        public EquipoDTO Equipo { get; set; }
    }
}
