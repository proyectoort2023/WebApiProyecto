using BDTorneus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.DTOs
{
    public class InscripcionDTO
    {
        public int TorneoId { get; set; }
        public TorneoDTO Torneo { get; set; }
        public int EquipoId { get; set; }
        public EquipoDTO Equipo { get; set; }
        public double Monto { get; set; }
        public string MedioPago { get; set; }
        public string Estado { get; set; }
    }
}
