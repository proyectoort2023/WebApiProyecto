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
    public class ValidadorEquipo : AbstractValidator<Equipo>
    {
        private readonly TorneoContext _db;
        public ValidadorEquipo(TorneoContext db)
        {
            _db = db;

            RuleFor(t => t.UsuarioId).NotEmpty();
            RuleFor(t => t.Nombre).MinimumLength(3).MaximumLength(30).WithMessage("El nombre debe tener entre 3 y 30 caracteres. ");
            RuleFor(t => t.Deporte).NotEmpty().NotNull().WithMessage("Falta la asignación del deporte del equipo");
            RuleFor(t => t.Abreviatura).MinimumLength(3).MaximumLength(10).WithMessage("El nombre de contacto debe tener entre 3 y 10 caracteres. ");
            RuleFor(t => t.Logo).NotEmpty().WithMessage("Debe seleccionar imagen para el logo. ");
            RuleFor(us => us.Nombre).NotEmpty().Must(NombreNoExiste).WithMessage("El nombre está duplicado");
            RuleFor(us => us.Abreviatura).NotEmpty().Must(AbreviaturaNoExiste).WithMessage("La abreviatura está duplicada");
            RuleFor(us => us.Abreviatura).NotEmpty().Must(AbreviaturaNoExiste).WithMessage("La abreviatura está duplicada");
            RuleFor(equipo => equipo.Jugadores).Must(DuplicadoJugador).WithMessage("Hay algun jugador duplicado en su selección"); ;
        }

        private bool NombreNoExiste(string nombre)
        {
            return !_db.Equipos.Any(c => c.Nombre == nombre.ToUpper().Trim());
        }

        private bool AbreviaturaNoExiste(string abreviatura)
        {
            return !_db.Equipos.Any(c => c.Abreviatura == abreviatura.ToUpper().Trim());
        }
        private bool DuplicadoJugador(List<Jugador> jugadores)
        {
            Dictionary<string, int> jugadorContador = new Dictionary<string, int>();

            foreach (var jugador in jugadores)
            {
                if (jugadorContador.ContainsKey(jugador.Cedula))
                {
                    jugadorContador[jugador.Cedula] += 1;
                }
                else
                {
                    jugadorContador.Add(jugador.Cedula, 1);
                }
            }
            bool valoresDuplicados = jugadorContador.Any(key => key.Value > 1);

            return !valoresDuplicados;
        }


    }
}
