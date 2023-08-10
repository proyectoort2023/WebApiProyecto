using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BDTorneus
{
    [DataContract(IsReference = true)]
    public class Torneo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Nombre { get; set; } = "";
        public DateTime Fecha { get; set; }
        public DateTime HoraComienzo { get; set; }
        public string Lugar { get; set; } = "";
        public string UbicacionGPS { get; set; } = "";
        public string NombreContacto { get; set; } = "";
        public string TelContacto { get; set; } = "";
        public string Logo { get; set; } = "";
        public string Banner { get; set; } = "";
        public double Precio { get; set; } 
        public string TipoPrecio { get; set; } = "";
        public string ConfiguracionEquipos { get; set; } = "";
        public int SetsMax { get; set; }
        public int PuntajeMax { get; set; }
        public int PuntajeMaxUltimoSet { get; set; }
        public int MaxEquiposInscriptos { get; set; }
        public int MaxJugadoresPorEquipo { get; set; }
        public bool Indoor { get; set; }
        public bool HabilitacionInscripcion { get; set; }
        public int CantidadCanchas { get; set; }
        public string Otros { get; set; } = "";
        public bool Suspendido { get; set; }       
        public bool Cerrrado { get; set; }
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; }
        public virtual List<Inscripcion> Inscripciones { get; set; } = new List<Inscripcion>();
        public virtual List<Partido> Fixture { get; set; } = new List<Partido>();

    }
}
