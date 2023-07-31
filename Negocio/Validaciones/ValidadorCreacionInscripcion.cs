using BDTorneus;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.Validaciones
{
    internal class ValidadorCreacionInscripcion : AbstractValidator<Inscripcion>
    {
        private readonly TorneoContext _db;
        public ValidadorCreacionInscripcion(TorneoContext db)
        {
            _db = db;

            RuleFor(ins => ins.UsuarioId).NotEmpty().NotNull().GreaterThan(0).WithMessage("El usuario no es correcto o no se encuentra");
            RuleFor(ins => ins.TorneoId).NotEmpty().NotNull().GreaterThan(0).WithMessage("El torneo no es correcto o no se encuentra");
            RuleFor(ins => ins.EquipoId).NotEmpty().NotNull().GreaterThan(0).WithMessage("El equipo no es correcto o no se encuentra");

            RuleFor(ins => ins).Must(InscripcionNoExiste).WithMessage("Ya inscribiste al equipo al torneo!!");
        }

       

        private bool InscripcionNoExiste(Inscripcion inscripcion)
        {
            return !_db.Inscripciones.Any(c => c.Equipo.Id == inscripcion.EquipoId &&
                                               c.Usuario.Id == inscripcion.UsuarioId &&
                                               c.Torneo.Id == inscripcion.TorneoId);
        }

      

    }
}
