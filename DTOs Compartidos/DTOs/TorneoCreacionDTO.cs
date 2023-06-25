using BDTorneus;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.DTOs
{
    public class TorneoCreacionDTO
    {
        [Required(ErrorMessage = "Obligatorio")]
        public string Nombre { get; set; }
        public DateTime Fecha { get; set; }
        public DateTime HoraComienzo { get; set; }
        [Required(ErrorMessage = "Obligatorio")]
        public string Lugar { get; set; }
        public string UbicacionGPS { get; set; }
        [Required(ErrorMessage = "Obligatorio")]
        public string NombreContacto { get; set; }
        [Required(ErrorMessage = "Obligatorio")]
        public string TelContacto { get; set; }
        [Required(ErrorMessage = "Obligatorio")]
        public string Logo { get; set; }
        [Required(ErrorMessage = "Obligatorio")]
        public string Banner { get; set; }
        [Required(ErrorMessage = "Obligatorio")]
        public double Precio { get; set; }
        [Required(ErrorMessage = "Obligatorio")]
        public string TipoPrecio { get; set; }
        [Required(ErrorMessage = "Obligatorio")]
        public string ConfiguracionEquipos { get; set; }
        [Required(ErrorMessage = "Obligatorio")]
        public int SetsMax { get; set; }
        [Required(ErrorMessage = "Obligatorio")]
        public int PuntajeMax { get; set; }
        [Required(ErrorMessage = "Obligatorio")]
        public int PuntajeMaxUltimoSet { get; set; }
        [Required(ErrorMessage = "Obligatorio")]
        public int MaxEquiposInscriptos { get; set; }
        [Required(ErrorMessage = "Obligatorio")]
        public int MaxJugadoresPorEquipo { get; set; }
        public bool Indoor { get; set; }
        public bool HabilitacionInscripcion { get; set; }
        [Required(ErrorMessage = "Obligatorio")]
        public int CantidadCanchas { get; set; }
        public string Otros { get; set; }
        public bool Suspendido { get; set; }
        public int UsuarioId { get; set; }
    }
}
