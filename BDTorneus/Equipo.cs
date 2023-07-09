using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDTorneus
{
    public class Equipo
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Deporte { get; set; }
        public string Abreviatura { get; set; } 
        public string Logo { get; set; }
        public string Capitan { get; set; }

        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; }
        public List<Inscripcion> Inscripciones { get; set; } = new List<Inscripcion>();
        public List<Jugador> Jugadores { get; set; } = new List<Jugador>();

    }
}
