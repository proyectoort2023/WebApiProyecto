using DTOs_Compartidos.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs_Compartidos.Models
{
    public class PlanilleroTorneoGrupo
    {
        public string NombreTorneo { get; set; }
        public List<AutorizacionPlanilleroDTO> PlanillasTorneos { get; set; }

    }
}
