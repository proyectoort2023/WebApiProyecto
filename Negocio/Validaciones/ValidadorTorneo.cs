﻿using BDTorneus;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilidades;

namespace Negocio.Validaciones
{
    public class ValidadorTorneo : AbstractValidator<Torneo>
    {
        private readonly TorneoContext _db;
        public ValidadorTorneo(TorneoContext db)
        {
            _db = db;

            RuleFor(t => t.Usuario).NotNull().NotEmpty().WithMessage("El usuario no se puede encontrar");
            RuleFor(t => t.Usuario.Id).NotEmpty();
            RuleFor(t => t.Nombre).MinimumLength(3).MaximumLength(50).WithMessage("El nombre debe tener entre 3 y 50 caracteres. ");
            RuleFor(t => t.Fecha).NotEmpty().NotNull().Must(ValidarFechaTorneo).WithMessage("La fecha del torneo debe ser mayor al dia de hoy.");
            RuleFor(t => t.NombreContacto).MinimumLength(3).MaximumLength(30).WithMessage("El nombre de contacto debe tener entre 3 y 30 caracteres. ");
            RuleFor(t => t.TelContacto).NotNull().NotEmpty().WithMessage("El telefono de contacto no puede estar vacio. ");
            RuleFor(t => t.Logo).NotEmpty().WithMessage("Debe seleccionar imagen para el logo. ");
            RuleFor(t => t.Banner).NotEmpty().WithMessage("Debe seleccionar imagen para el banner. ");
            RuleFor(t => t.Precio).GreaterThan(0).WithMessage("El precio no puede ser cero. ");
            RuleFor(t => t.TipoPrecio).Must(ValidarTipoDePrecio).WithMessage("No se ha seleccionado ningun tipo de precio. ");
            RuleFor(t => t.ConfiguracionEquipos).Must(ValidarConfiguracionEquipo).WithMessage("La modalidad elegida no coincide. ");
            RuleFor(t => t.SetsMax).InclusiveBetween(1, 5).WithMessage("Set maximo de 5.");
            RuleFor(t => t.PuntajeMax).InclusiveBetween(1,25).WithMessage("Puntaje por set entre 1 y 25 puntos. ");
            RuleFor(t => t.PuntajeMaxUltimoSet).InclusiveBetween(1, 25).WithMessage("Puntaje por set entre 1 y 25 puntos. ");
            RuleFor(t => t.MaxEquiposInscriptos).InclusiveBetween(4, 9999).WithMessage("Cantidad de inscriptos deben ser entre 4 y 1000. ");
            RuleFor(t => t.MaxJugadoresPorEquipo).InclusiveBetween(6, 12).WithMessage("La cantidad de jugadores por equipo deben ser entre 6 y 12. ");
            RuleFor(t => t.CantidadCanchas).GreaterThan(0).WithMessage("cantidad de canchas deben ser de uno en adelante. ");

        }
       
        private bool ValidarFechaTorneo(DateTime date)
        {
            bool fechaEsMayor = date > DateTime.Today;
            return fechaEsMayor;
        }
        private bool ValidarTipoDePrecio(string tipoPrecio)
        {
            bool valorEcontrado = Util.TipoPrecioDiccionario.Any(value => value.Value == tipoPrecio);
            return valorEcontrado;
        }
        private bool ValidarConfiguracionEquipo(string tipo)
        {
            bool valorEcontrado = Util.ConfigEquiposDiccionario.Any(value => value.Key == tipo);
            return valorEcontrado;
        }
    }
}
