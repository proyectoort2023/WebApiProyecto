﻿using BDTorneus;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.DTOs
{
    public class TorneoCreacionDTO
    {
        [Required(ErrorMessage = "Obligatorio")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "Obligatorio")]
        public DateTime Fecha { get; set; }
        [Required(ErrorMessage = "Obligatorio")]
        public DateTime HoraComienzo { get; set; }
        [Required(ErrorMessage = "Obligatorio")]
        public string Lugar { get; set; }
        public string UbicacionGPS { get; set; }
        [Required(ErrorMessage = "Obligatorio")]
        public string NombreContacto { get; set; }
        [Required(ErrorMessage = "Obligatorio")]
        [RegularExpression(@"^\d{9}$", ErrorMessage = "El n° de celular no es correcto")]
        public string TelContacto { get; set; }
        [Required(ErrorMessage = "Obligatorio")]
        public string Logo { get; set; }
        [Required(ErrorMessage = "Obligatorio")]
        public string Banner { get; set; }
        [Required(ErrorMessage = "Obligatorio")]
        [Range(1, double.MaxValue, ErrorMessage = "El número debe ser mayor a 0")]
        public double Precio { get; set; }
        [Required(ErrorMessage = "Obligatorio")]
        public string TipoPrecio { get; set; }
        [Required(ErrorMessage = "Obligatorio")]
        public string ConfiguracionEquipos { get; set; }
        [Required(ErrorMessage = "Obligatorio")]
        [Range(1, 5, ErrorMessage = "El número debe estar entre 1 y 5")]
        public int SetsMax { get; set; }
        [Required(ErrorMessage = "Obligatorio")]
        [Range(1, 5, ErrorMessage = "El número debe estar entre 1 y 5")]
        public int PuntajeMax { get; set; }
        [Required(ErrorMessage = "Obligatorio")]
        [Range(1, 25, ErrorMessage = "El número debe estar entre 1 y 25")]
        public int PuntajeMaxUltimoSet { get; set; }
        [Required(ErrorMessage = "Obligatorio")]
        [Range(1, 25, ErrorMessage = "El número debe estar entre 1 y 25")]
        public int MaxEquiposInscriptos { get; set; }
        [Required(ErrorMessage = "Obligatorio")]
        [Range(1, 1000, ErrorMessage = "El número debe ser mayor a 0")]
        public int MaxJugadoresPorEquipo { get; set; }
        public bool Indoor { get; set; }
        public bool HabilitacionInscripcion { get; set; }
        [Required(ErrorMessage = "Obligatorio")]
        [Range(1, double.MaxValue, ErrorMessage = "El número debe ser mayor a 0")]
        public int CantidadCanchas { get; set; }
        public string Otros { get; set; }
        public bool Suspendido { get; set; }
        public int UsuarioId { get; set; }
    }
}
