using BDTorneus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.DTOs
{
    public class EquipoDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Deporte { get; set; }
        public string Abreviatura { get; set; }
        public string Logo { get; set; }

        public int UsuarioId { get; set; }
        public UsuarioDTO Usuario { get; set; }
        public List<InscripcionDTO> Inscripciones { get; set; } = new List<InscripcionDTO>();
    }
}
