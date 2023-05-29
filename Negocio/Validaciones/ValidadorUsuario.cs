using BDTorneus;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.Validaciones
{
    public class ValidadorUsuarioRegistro : AbstractValidator<Usuario>
    {
        public ValidadorUsuarioRegistro()
        {
            RuleFor(us => us.Mail).NotNull().NotEmpty().WithMessage("mail vacío");
            RuleFor(us => us.Pass).NotEmpty().WithMessage("contrasela vacía");
            RuleFor(us => us.Rol).NotEmpty().NotNull().WithMessage("No ha seleccionado un rol");

        }

    }
}
