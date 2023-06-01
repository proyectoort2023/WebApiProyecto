using BDTorneus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.DTOs
{
    public class TorneoCreacionDTO
    {
        public string Nombre { get; set; }
        public DateTime Fecha { get; set; }
        public DateTime HoraComienzo { get; set; }
        public string Lugar { get; set; }
        public string UbicacionGPS { get; set; }
        public string NombreContacto { get; set; }
        public string TelContacto { get; set; }
        public string Logo { get; set; }
        public string Banner { get; set; }
        public double Precio { get; set; }
        public string TipoPrecio { get; set; }
        public string ConfiguracionEquipos { get; set; }
        public int SetsMax { get; set; }
        public int PuntajeMax { get; set; }
        public int PuntajeMaxUltimoSet { get; set;}
        public int MaxEquiposInscriptos { get; set; }
        public int MaxJugadoresPorEquipo { get; set; }
        public bool Indoor { get; set; }
        public bool HabilitacionInscripcion { get; set; }
        public int CantidadCanchas { get; set; }
        public string Otros { get; set; }
        public bool Suspendido { get; set; }
        public int UsuarioId { get; set; }
    }
}
