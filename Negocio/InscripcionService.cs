using BDTorneus;
using DTOs_Compartidos.DTOs;
using FluentValidation.Results;
using MercadoPago.Config;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Negocio.DTOs;
using Negocio.Validaciones;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilidades;
using MercadoPago.Resource.Payment;

namespace Negocio
{
    public class InscripcionService
    {

        private readonly TorneoContext _db;

        public InscripcionService(TorneoContext db)
        {
            _db = db;
        }



        public async Task<List<Inscripcion>> ObtenerInscripcionesSegunUsuario(int usuarioId)
        {
            try
            {
                if (usuarioId < 0) throw new Exception("El usuario no se encuentra para obtener las inscripciones");

                List<Inscripcion> inscripciones = await _db.Inscripciones.Include(i => i.Equipo)
                                                                          .Include(i => i.Torneo)
                                                                          .Where(w => w.Usuario.Id == usuarioId).ToListAsync();

                if (inscripciones == null) throw new Exception("No hay inscricpiones válidas");

                return inscripciones;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<Inscripcion>> ObtenerInscripcionesSegunTorneo(int torneoId)
        {
            try
            {
                if (torneoId < 1) throw new Exception("El torneo no se encuentra para obtener las inscripciones");

                List<Inscripcion> inscripciones = await _db.Inscripciones.Include(i => i.Equipo)
                                                                          .Include(i => i.Torneo)
                                                                          .Include(i => i.Usuario)
                                                                          .Where(w => w.Torneo.Id == torneoId)
                                                                          .ToListAsync();

                inscripciones.ForEach(ins => {
                    ins.Usuario.Pass = "";
                    ins.Usuario.AccessTokenMercadopago = "";
                    ins.Usuario.AccessTokenRefreshMercadopago = "";
                });

                if (inscripciones == null) throw new Exception("No hay inscricpiones válidas");

                return inscripciones;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task<Inscripcion> AgregarNuevaInscripcion(Inscripcion inscripcion)
        {
            try
            {
                string mensajeError = "";

                ValidadorCreacionInscripcion validacion = new(_db);
                ValidationResult resultValidacion = validacion.Validate(inscripcion);
                if (!resultValidacion.IsValid)
                {
                    resultValidacion.Errors.ForEach(f => mensajeError += f.ErrorMessage);
                    throw new Exception(mensajeError);
                }

                ValidadorJugRepetidosTorneoMismoDia validarJugadoresRepetidos = new(_db);

                bool estaRepetido = await validarJugadoresRepetidos.BuscarRepetidos(inscripcion);

                if (estaRepetido) throw new Exception("Algun jugador se repite en otro equipo este dia");

                inscripcion.Usuario = await _db.Usuarios.FindAsync(inscripcion.UsuarioId);
                inscripcion.Torneo = await _db.Torneos.FindAsync(inscripcion.TorneoId);
                inscripcion.Equipo = await _db.Equipos.FindAsync(inscripcion.EquipoId);

                _db.Entry(inscripcion.Usuario).State = EntityState.Unchanged;
                _db.Entry(inscripcion.Torneo).State = EntityState.Unchanged;
                _db.Entry(inscripcion.Equipo).State = EntityState.Unchanged;


                var inscrpcionNueva = await _db.Inscripciones.AddAsync(inscripcion);

                await _db.SaveChangesAsync();

                return inscrpcionNueva.Entity;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task<bool> ActualizarPagoEfectivo(string estado, int inscripcionId)
        {
            try
            {
                Inscripcion inscripcion = await _db.Inscripciones.SingleOrDefaultAsync(ins => ins.Id == inscripcionId);
                if (inscripcion == null) throw new Exception("No existe la inscripcion");

                inscripcion.MedioPago = Util.MedioPago.EFECTIVO.ToString();
                inscripcion.Estado = estado;    

                int actualizados = await _db.SaveChangesAsync();
                return actualizados > 0;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public async Task<bool> ActualizarPagoMercadoPago(PreferenciaMercadopagoDTO preferencia)
        {
            try
            { //soalamente cuando fue aprobado el pago
                Inscripcion inscripcion = await _db.Inscripciones.SingleOrDefaultAsync(ins => ins.Id == preferencia.InscripcionId);
                if (inscripcion == null) throw new Exception("No existe la referencia de pago");

                inscripcion.MedioPago = Util.MedioPago.MERCADOPAGO.ToString();
                inscripcion.Estado = Util.EstadoPago.PAGADO.ToString();
                inscripcion.PreferenciaMP = preferencia.PreferenciaId;
                inscripcion.OrdenPagoMP = preferencia.OrdenPagoId;

                int actualizados = await _db.SaveChangesAsync();
                if  (actualizados == 0) throw new Exception("No se ha podido actualiza el pago. W81");

                return actualizados > 0;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public async Task<bool> BajaInscripcion(int inscripcionId, string accessTokenMercadopago)
        {
            try
            { //soalamente cuando fue aprobado el pago
                string ordenpagoId = "";
                decimal montoReembolso = 0;
                Inscripcion inscripcionBuscada = await _db.Inscripciones.Include(inc => inc.Torneo)
                                                                        .SingleOrDefaultAsync(ins => ins.Id == inscripcionId);
                if (inscripcionBuscada == null) throw new Exception("No existe la la isncripcion buscada. W91");

               
                if (DateTime.Today.Date > inscripcionBuscada.Torneo.Fecha.Date.AddDays(-2)) throw new Exception("Puede dar de baja la inscripción hasta 2 dias antes del comienzo del torneo");

                _db.Inscripciones.Remove(inscripcionBuscada);

                int actualizados = await _db.SaveChangesAsync();

                if (inscripcionBuscada.MedioPago == Util.MedioPago.MERCADOPAGO.ToString())
                {
                    ordenpagoId = inscripcionBuscada.OrdenPagoMP;
                    montoReembolso = (decimal)inscripcionBuscada.Monto;
                    await ReembolsoInscripción(ordenpagoId, montoReembolso, accessTokenMercadopago);
                }

                return actualizados > 0;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public async Task<bool> EstaEnLimiteInscriptos(int idTorneo)
        {
            try
            {
                Torneo torneo = await _db.Torneos.SingleOrDefaultAsync(w => w.Id == idTorneo);

                int cantidadInscripciones = await _db.Inscripciones.CountAsync(co => co.Torneo.Id == idTorneo);

                return cantidadInscripciones >= torneo.MaxEquiposInscriptos;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }



        public async Task ReembolsoInscripción(string ordenpagoId, decimal montoReembolso, string accessToken)
        {
            try
            {
                MercadoPagoConfig.AccessToken = accessToken;

                long ordenId = long.Parse(ordenpagoId);

                var client = new MercadoPago.Client.Payment.PaymentRefundClient();
                PaymentRefund refund = await client.RefundAsync(ordenId, montoReembolso);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }




    }
 }
