using BDTorneus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.DTOs
{
    public class UsuarioDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Mail { get; set; }
        public string Pass { get; set; }
        public string Rol { get; set; }
        public string Tel { get; set; }
        public string Token { get; set; }

        public List<Equipo> Equipos { get; set; } = new List<Equipo>();
        public List<Torneo> Torneos { get; set; } = new List<Torneo>();
    }
}
