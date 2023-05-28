﻿using BDTorneus;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.Validaciones
{
    public class ValidadorActualizacionTorneo : AbstractValidator<Torneo> 
    {
        private readonly TorneoContext _db;
        public ValidadorActualizacionTorneo(TorneoContext db)
        {
            _db = db;

            RuleFor(t => t.Nombre).MinimumLength(3).MaximumLength(40).WithMessage("El nombre debe tener entre 3 y 40 caracteres. ");
            RuleFor(t => t.Nombre).NotEmpty().Must(TorneoNoExiste).WithMessage("El nombre de torneo que quiere registrar ya existe. ");
            RuleFor(t => t.Fecha).NotEmpty().NotNull().Must(ValidarFechaTorneo).WithMessage("La fecha del torneo debe ser al dia de hoy. ");
            RuleFor(t => t.NombreContacto).MinimumLength(3).MaximumLength(30).WithMessage("El nombre de contacto debe tener entre 3 y 30 caracteres. ");
            RuleFor(t => t.TelContacto).NotNull().NotEmpty().WithMessage("El telefono de contacto no puede estar vacio. ");
            RuleFor(t => t.Logo).NotEmpty().WithMessage("Debe seleccionar imagen para el logo. ");
            RuleFor(t => t.Banner).NotEmpty().WithMessage("Debe seleccionar imagen para el banner. ");
            RuleFor(t => t.Precio).GreaterThan(0).WithMessage("El precio no puede ser cero. ");
            RuleFor(t => t.TipoPrecio).Must(ValidarTipoDePrecio).WithMessage("No se ha seleccionado ningun tipo de precio. ");
            RuleFor(t => t.ConfiguracionEquipos).Must(ValidarConfiguracionEquipo).WithMessage("La modalidad elegida no coincide. ");
            RuleFor(t => t.SetsMax).InclusiveBetween(1, 5).WithMessage("Set maximo de 5. ");
            RuleFor(t => t.PuntajeMax).InclusiveBetween(5, 25).WithMessage("Puntaje por set entre 5 y 25 puntos. ");
            RuleFor(t => t.PuntajeMaxUltimoSet).InclusiveBetween(5, 25).WithMessage("Puntaje por set entre 5 y 25 puntos. ");
            RuleFor(t => t.MaxEquiposInscriptos).InclusiveBetween(4, 1000).WithMessage("Cantidad de inscriptos deben ser entre 4 y 1000. ");
            RuleFor(t => t.MaxJugadoresPorEquipo).InclusiveBetween(6, 12).WithMessage("La cantidad de jugadores por equipo deben ser entre 6 y 12. ");
            RuleFor(t => t.CantidadCanchas).GreaterThan(0).WithMessage("cantidad de canchas deben ser de una en adelante. ");

        }

        private bool TorneoNoExiste(string nombre)
        {
            return !_db.Torneos.Any(c => c.Nombre.ToUpper() == nombre.ToUpper().Trim());
        }
        private bool ValidarFechaTorneo(DateTime date)
        {
            return date > DateTime.Now;
        }
        private bool ValidarTipoDePrecio(string tipoPrecio)
        {
            return tipoPrecio == "JUGADOR" || tipoPrecio == "EQUIPO";
        }
        private bool ValidarConfiguracionEquipo(string tipo)
        {
            string[] modalidades = { "HOMBRES", "MIXTO 5+1", "MIXTO 5+2", "MUJERES" };
            return modalidades.Contains(tipo);
        }
    }
}
