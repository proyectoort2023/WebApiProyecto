using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.DTOs
{
    public class JugadorDTO
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Obligatorio")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Debe tener entre 3 y 25 caracteres")]
        public string NombreCompleto { get; set; }
        [RegularExpression(@"[0-9]+$", ErrorMessage = "La cédula debe ser numerica sin puntos ni guiones")]
        public string Cedula { get; set; }
        public DateTime FechaNacimiento { get; set; }
    }
}
