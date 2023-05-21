using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDTorneus
{ 
    public class Notificacion
    {
        public int Id { get; set; }
        public int TorneoId { get; set; }
        public int EquipoId { get; set; }
        public string Mensaje { get; set; }
        public bool Leido { get; set; }
        public bool General { get; set; }
        public DateTime FechaHora  { get; set; }
        public Torneo Torneo { get; set; }
        public Equipo Equipo { get; set; }
    }
}
