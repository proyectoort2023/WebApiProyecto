using BDTorneus;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.Validaciones
{
    public class ValidadorUsuarioDuplicado : AbstractValidator<Usuario>
    {
        private readonly TorneoContext _db;
        public ValidadorUsuarioDuplicado(TorneoContext db)
        {
            _db = db;

            RuleFor(us => us.Mail).NotEmpty().Must(UsuarioNoExiste).WithMessage("DUPLICADO");

        }

        private bool UsuarioNoExiste(string mail)
        {
            return !_db.Usuarios.Any(c => c.Mail == mail.ToLower().Trim());
        }
    }
}
