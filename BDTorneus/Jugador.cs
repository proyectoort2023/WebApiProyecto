using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDTorneus
{
    public class Jugador
    {
        public int Id { get; set; }
        public string NombreCompleto { get; set; }
        public string Cedula { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public List<Equipo> Equipos { get; set; }
    }
}
