using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.DTOs
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "Obligatorio")]
        [RegularExpression(@"^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$", ErrorMessage = "El correo no es válido.")]
        public string Mail { get; set; }
        [Required(ErrorMessage = "Obligatorio")]
        [StringLength(30, ErrorMessage = "Minimo 5 caracteres", MinimumLength = 5)]
        public string Pass { get; set; }
    }
}
