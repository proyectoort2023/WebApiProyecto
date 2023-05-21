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
        private readonly TorneoContext _db;
        public ValidadorUsuarioRegistro(TorneoContext db)
        {
            _db =db;

            RuleFor(us => us.Mail).NotNull().NotEmpty().WithMessage("mail vacío");
            RuleFor(us => us.Pass).NotEmpty().WithMessage("contrasela vacía");
            RuleFor(us => us.Rol).NotEmpty().NotNull().WithMessage("No ha seleccionado un rol");
            RuleFor(us => us.Mail).NotEmpty().Must(UsuarioNoExiste).WithMessage("El mail que quiere registrar ya existe");

        }

        private bool UsuarioNoExiste(string mail)
        {
            return !_db.Usuarios.Any(c => c.Mail == mail.ToLower().Trim());
        }
    }
}
