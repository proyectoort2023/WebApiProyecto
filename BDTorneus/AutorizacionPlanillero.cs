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
        public int UsuarioOrganizadorId { get; set; }
        public Usuario UsuarioOrganizador { get; set; }
        public int UsuarioPlanilleroId { get; set; }
        public Usuario UsuarioPlanillero { get; set; }
        public int TorneoId { get; set; }
        public Torneo Torneo { get; set; }
    }
}
