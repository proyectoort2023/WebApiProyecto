using BDTorneus;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Negocio.Validaciones
{
    public class ValidacionEquipo : AbstractValidator<Equipo>
    {
        private readonly TorneoContext _db;
        public ValidacionEquipo(TorneoContext db)
        {
            _db = db;

            RuleFor(t => t.Usuario.Id).NotEmpty();
            RuleFor(t => t.Nombre).MinimumLength(3).MaximumLength(30).WithMessage("El nombre debe tener entre 3 y 30 caracteres. ");
            RuleFor(t => t.Deporte).NotEmpty().NotNull().WithMessage("Falta la asignación del deporte dle equipo");
            RuleFor(t => t.Abreviatura).MinimumLength(3).MaximumLength(6).WithMessage("El nombre de contacto debe tener entre 3 y 6 caracteres. ");
            RuleFor(t => t.Logo).NotEmpty().WithMessage("Debe seleccionar imagen para el logo. ");
            RuleFor(us => us.Nombre).NotEmpty().Must(NombreNoExiste).WithMessage("El nombre está duplicado");
            RuleFor(us => us.Abreviatura).NotEmpty().Must(AbreviaturaNoExiste).WithMessage("La abreviatura está duplicada");
        }

        private bool NombreNoExiste(string nombre)
        {
            return !_db.Equipos.Any(c => c.Nombre == nombre.ToUpper().Trim());
        }

        private bool AbreviaturaNoExiste(string abreviatura)
        {
            return !_db.Equipos.Any(c => c.Abreviatura == abreviatura.ToUpper().Trim());
        }


    }
}
