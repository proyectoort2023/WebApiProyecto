using BDTorneus;
using DTOs_Compartidos.DTOs;
using Microsoft.AspNetCore.Components;
using Microsoft.Identity.Client;
using Negocio.DTOs;
using System.Drawing;
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

        private async Task CargarInscripciones()
        {
            try
            {
                int usuarioId = _usuarioServicio.ObtenerUsuarioLogueado().Id;
                _inscripciones = await _inscripcionServicio.ObtenerInscripcionesDeUsuario(usuarioId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<List<InscripcionDTO>> ObtenerInscripciones()
        {
          if (_inscripciones.Count == 0)
            {
                await CargarInscripciones();
            }
          return _inscripciones;
        }

        public void SetInscripcionSeleccionado(int inscripcionId)
        {
            _inscripcionSeleccionado = _inscripciones.SingleOrDefault(ins => ins.Id == inscripcionId);
        }

        public async Task<List<InscripcionDTO>> ObtenerInscripcionesOrganizador(int torneoId)
        {
            _inscripciones = await _inscripcionServicio.ObtenerInscripcionesTorneo(torneoId);
            return _inscripciones;
        }


        public InscripcionDTO ObtenerInscripcionActual()
        {
            return _inscripcionSeleccionado;
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
                    Estado = Util.EstadoPago.NO_SELECCIONADO.ToString(),
                    MedioPago = "",
                    OrdenPagoMP = "",
                    PreferenciaMP = ""
                };

                InscripcionDTO nuevaInscripcion = await _inscripcionServicio.RegistrarNuevaInscripcion(inscripcion);

                if (nuevaInscripcion == null) throw new Exception("No se ha podido realizar la inscripcion");

                _inscripciones.Add(nuevaInscripcion);
                _inscripcionSeleccionado = nuevaInscripcion;

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


        public async Task<bool> ActualizarMedioPagoEfectivo(string estado)
        {
            try
            {
                MedioPagoEfectivoDTO inscripcionEf = new()
                {
                    Estado = estado,
                    InscripcionId = _inscripcionSeleccionado.Id
                };

                bool resultado = await _inscripcionServicio.ActualizarDatosPagoEfetivo(inscripcionEf);
                //await CargarInscripciones();
                if (!resultado) throw new Exception("No se ha podido actualizar");

                int posicion = BuscarIndexInscripcion(inscripcionEf.InscripcionId);
                ActualizarEstadoInscripcionPorIndex(posicion, inscripcionEf.Estado);
                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> ActualizarMercadoPago(PreferenciaMercadopagoDTO preferenciaMP)
        {
            try
            {
                bool resultado = await _inscripcionServicio.ActualizarDatosPagoMercadopago(preferenciaMP);
                await CargarInscripciones();
                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

     
        private void ActualizarEstadoInscripcionPorIndex(int posicion, string estado)
        {
            InscripcionDTO inscripcionActualizar = _inscripciones[posicion];
            inscripcionActualizar.MedioPago = Util.MedioPago.EFECTIVO.ToString();
            inscripcionActualizar.Estado = estado;
        }
        private int BuscarIndexInscripcion(int inscripcionId)
        {
            int posicion = _inscripciones.FindIndex(find => find.Id == inscripcionId);
            return posicion;
        }

        public async Task<bool> BajaInscripcion(int inscripcionId)
        {
            try
            {
                bool resultado = await _inscripcionServicio.BajaInscripcion(inscripcionId);
                if (resultado)
                {
                    _inscripciones.RemoveAll(rem => rem.Id == inscripcionId);
                }
                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }



    }
}
