using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.DTOs
{
    public class RegistroDTO
    {
        [Required(ErrorMessage = "Obligatorio")]
        [MinLength(3, ErrorMessage = "Minimo 3 caracteres")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Obligatorio")]
        //[RegularExpression(@"^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$", ErrorMessage = "El correo no es válido.")]
        [EmailAddress(ErrorMessage = "El correo no es válido")]
        public string Mail { get; set; }

        [Required(ErrorMessage = "Obligatorio")]
        [StringLength(30, ErrorMessage = "Minimo 5 caracteres", MinimumLength = 5)]
        public string Pass { get; set; }

        [Required(ErrorMessage = "Obligatorio")]
        [Compare(nameof(Pass),ErrorMessage ="Las contraseñas no son iguales")]
        public string PassRepeticion { get; set; }

        [Required(ErrorMessage = "Obligatorio")]
        public string Rol { get; set; }

        [Required(ErrorMessage = "Obligatorio")]
        [RegularExpression(@"^\d{9}$", ErrorMessage = "El n° de celular no es correcto")]
        public string Tel { get; set; }

        public string IdUsuarioGoogle { get; set; }
    }
}
