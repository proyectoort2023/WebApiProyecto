﻿using Microsoft.AspNetCore.Components;
using Negocio.DTOs;
using System.Linq.Expressions;
using TorneusClienteWeb.Servicios_de_Datos;
using Utilidades;

namespace TorneusClienteWeb.Servicios
{
    public class InscripcionServicio
    {
        [Inject] private TorneoServicio _torneoServicio { get; set; }
        [Inject] private UsuarioServicio _usuarioServicio { get; set; }
        [Inject] private InscripcionServicioDatos _inscripcionServicio { get; set; }

        private List<InscripcionDTO> _inscripciones { get; set; } = new List<InscripcionDTO>();
        private InscripcionDTO _inscripcionSeleccionado;

        public InscripcionServicio(TorneoServicio torneoServicio, UsuarioServicio usuarioServicio, InscripcionServicioDatos inscripcionServicio)
        {
            _torneoServicio = torneoServicio;
            _usuarioServicio = usuarioServicio;
            _inscripcionServicio = inscripcionServicio;
        }

        private async Task CargaInscripciones()
        {
            //por web api inicial
        }
        public async Task<List<InscripcionDTO>> ObtenerInscripciones()
        {
          if (_inscripciones.Count == 0)
            {
                await CargaInscripciones();
            }
          return _inscripciones;
        }

        public async Task AgregarInscripcionNueva(int equipoId, double precio, string tipoPrecio, int cantidadJugadores)
        {
            try
            {
                InscripcionDTO inscripcion = new()
                {
                    UsuarioId = _usuarioServicio.ObtenerUsuarioLogueado().Id,
                    TorneoId = _torneoServicio.ObtenerTorneoActual().Id,
                    EquipoId = equipoId,
                    Monto = CalculoMontoAPagar(precio, tipoPrecio, cantidadJugadores),
                    Estado = Util.EstadoPago.PENDIENTE.ToString(),
                    MedioPago = "",
                    OrdenPagoMP = "",
                    PreferenciaMP = ""
                };

                InscripcionDTO nuevaInscripcion = await _inscripcionServicio.RegistrarNuevaInscripcion(inscripcion);

                _inscripciones.Add(nuevaInscripcion);


            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
          



        }

        private double CalculoMontoAPagar(double precio, string tipoPrecio, int cantidadJugadores)
        {
            double total = tipoPrecio == Util.TIPO_PRECIO_POR_EQUIPO ? precio : precio * cantidadJugadores;
            return total;

        }

        public bool ExisteInscripcion(int torneoId) => _inscripciones.Any(a => a.TorneoId == torneoId);







    }
}
