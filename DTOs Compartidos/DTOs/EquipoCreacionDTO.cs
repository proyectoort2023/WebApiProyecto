using Negocio.DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs_Compartidos.DTOs
{
    public class EquipoCreacionDTO
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Obligatorio")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Debe tener entre 3 y 30 caracteres")]
        public string Nombre { get; set; }
        public string Deporte { get; set; } = "VOLEY";
        [Required(ErrorMessage = "Obligatorio")]
        [StringLength(6, MinimumLength = 3, ErrorMessage = "Debe tener entre 3 y 6 caracteres")]
        public string Abreviatura { get; set; }
        [Required(ErrorMessage = "Obligatorio")]
        public string Logo { get; set; }

        [Required(ErrorMessage = "Obligatorio")]
        public int UsuarioId { get; set; }
        public UsuarioDTO Usuario { get; set; }
        public List<JugadorDTO> Jugadores { get; set; } = new List<JugadorDTO>();
    }
}
