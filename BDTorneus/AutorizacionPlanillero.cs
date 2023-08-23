using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDTorneus
{
    public class AutorizacionPlanillero
    {
        public int Id { get; set; }
        public int UsuarioIdOrganizadorId { get; set; }
        public Usuario UsuarioIdOrganizador { get; set; }
        public int UsuarioIdPlanilleroId { get; set; }
        public Usuario UsuarioIdPlanillero { get; set; }
        public int TorneoId { get; set; }
        public Torneo Torneo { get; set; }
    }
}
