using BDTorneus;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.Validaciones
{
    public class ValidarTorneoDuplicado : AbstractValidator<Torneo>
    {
        private readonly TorneoContext _db;

        public ValidarTorneoDuplicado(TorneoContext db)
        {
            _db = db;
            RuleFor(t => t.Nombre).NotEmpty().Must(TorneoNoExiste).WithMessage("DUPLICADO");
        }

        private bool TorneoNoExiste(string nombre)
        {
            return !_db.Torneos.Any(c => c.Nombre.ToUpper() == nombre.ToUpper().Trim());
        }
    }
}
