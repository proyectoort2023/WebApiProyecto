using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDTorneus
{
    public class Medalla
    {
        public int Id { get; set; }
        public string NombreTorneo { get; set; }
        public string MedallaImage { get; set; }
        public int EquipoId { get; set; }
        public DateTime Fecha { get; set; }
        public string Tipo { get; set; }
    }
}
