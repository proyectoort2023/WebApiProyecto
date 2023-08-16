using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs_Compartidos.Models
{
    public class PlanilleroTorneo
    {
        public int UsuarioIdOrganizador { get; set; }
        public int UsuarioIdPlanillero { get; set; }
        public int TorneoId { get; set; }
        public DateTime FechaTorneo { get; set; }
        public string NombreTorneo { get; set; }
    }
}
