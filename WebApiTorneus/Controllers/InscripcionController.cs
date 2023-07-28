using AutoMapper;
using Azure.Core;
using BDTorneus;
using DTOs_Compartidos.DTOs;
using DTOs_Compartidos.Models;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Negocio;
using Negocio.DTOs;
using Negocio.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using WebApiTorneus.HubSignalR;
using WebApiTorneus.Services;
using static Utilidades.Util;

namespace WebApiTorneus.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InscripcionController : Controller
    {
        private readonly IMapper _mapper;
        private readonly InscripcionService _inscripcionService;
        private readonly TorneoService _torneoService;
        private IHubContext<TorneusHub> _torneoHub;
        private readonly IConfiguration _config;


        public InscripcionController(IMapper mapper, UsuarioService usuarioService, InscripcionService inscripcionService, 
                                     IHubContext<TorneusHub> torneoHub, IConfiguration config, TorneoService torneoService)
        {
            _mapper = mapper;
            _inscripcionService = inscripcionService;
            _torneoHub = torneoHub;
            _config = config;
            _torneoService = torneoService;
        }


        /// <summary>
        /// Permite a un usuario de equipo agregar una inscripcion de un equipo a un torneo
        /// </summary>
        /// <returns> Agreaga una nueva inscripción a un torneo</returns>
        /// <remarks>
        /// Agreaga una nueva inscripción a un torneo
        /// </remarks>
        /// <response code="200">OK. se ha inscripto el equipo al torneos</response>
        /// <response code="400">No encontrado</response>
        [ProducesResponseType(typeof(TokenModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "EQUIPO")]
        [HttpPost("Agregar")]
        public async Task<IActionResult> AgregarInscripcionNueva([FromBody] InscripcionDTO inscripcionDTO)
        {
            try
            {
                Inscripcion inscripcion = _mapper.Map<InscripcionDTO, Inscripcion>(inscripcionDTO);

                int torneoId = inscripcionDTO.TorneoId;

                Inscripcion inscripcionNueva = await _inscripcionService.AgregarNuevaInscripcion(inscripcion);
                InscripcionDTO inscripcionNuevaDTO = _mapper.Map<Inscripcion, InscripcionDTO>(inscripcionNueva);

                //necesito corroborar si se llegó al limite de inscriptos, actualizar el mensaje y enviar el cierre si es necesario
                bool estaEnLimiteInscripcion = await _inscripcionService.EstaEnLimiteInscriptos(torneoId);

                if (estaEnLimiteInscripcion)
                {
                   bool resultado = await _torneoService.CerrarInscripciones(torneoId);
                    if (resultado) await _torneoHub.Clients.All.SendAsync("RecibidorAperturaCierreTorneo", torneoId, false);
                }
                return Ok(inscripcionNuevaDTO);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        /// <summary>
        /// Permite obtener las inscripciones de un usuario con rol Equipo
        /// </summary>
        /// <remarks>
        /// Este endpoint permite obtener las inscripciones de un usuario con rol Equipo
        /// </remarks>
        /// <response code="200">OK. Listado obtenido</response>
        /// <response code="400">Validaciones varias erroneas</response>
        [ProducesResponseType(typeof(List<Inscripcion>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet("Listado/{usuarioId}")]
        public async Task<IActionResult> GetInscripconesSegunUsuario(int usuarioId)
        {
            try
            {
                var inscripcionListado = await _inscripcionService.ObtenerInscripcionesSegunUsuario(usuarioId);
                List<InscripcionDTO> inscripcionesDTO = _mapper.Map <List<Inscripcion>,List <InscripcionDTO>>(inscripcionListado);

                return Ok(inscripcionesDTO);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        /// <summary>
        /// Notificaciones de estado de pago de Mercado Pago
        /// </summary>
        /// <returns> retorna Ok segun requerimiento de Mercado Pago</returns>
        /// <remarks>
        /// Recibe notofocación de pago de Mercado Pago API y notifica a través de SignalR al usuario que realizó el pago de la suscripción
        /// </remarks>
        /// <response code="200">OK</response>
        [ProducesResponseType(typeof(TokenModel), StatusCodes.Status200OK)]
        [HttpPost("Notificacion/Mercadopago")]
        public async Task<IActionResult> WebHooksMercadoPago()
        {
            try
            {
                using (StreamReader reader = new StreamReader(Request.Body))
                {
                    string jsonString = await reader.ReadToEndAsync();

                    WebHook webhookData = JsonConvert.DeserializeObject<WebHook>(jsonString);

                    await _torneoHub.Clients.All.SendAsync("RecibidorNotificacionMercadoPago", webhookData);
                }


                    return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }






        /// <summary>
        /// Permite a un usuario de equipo u organizador actualizar los datos del medio de pago efectivo
        /// </summary>
        /// <returns> bool: verdadero si se actualizaron los datos</returns>
        /// <remarks>
        /// Actualiza el campo medio de pago y el estado enviado (pendiente o pagado)
        /// </remarks>
        /// <response code="200">OK. Se actualizaron los datos</response>
        /// <response code="400">No encontrado</response>
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "EQUIPO,ORGANIZADOR")]
        [HttpPost("MedioPago/Efectivo")]
        public async Task<IActionResult> ActualizarMedioPagoEfectivo([FromBody] MedioPagoEfectivoDTO inscripcionEf)
        {
            try
            {
                bool actualizarDatosPago = await _inscripcionService.ActualizarPagoEfectivo(inscripcionEf.Estado, inscripcionEf.InscripcionId);

                return Ok(actualizarDatosPago);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        /// <summary>
        /// Permite a un usuario de equipo actualizar los datos del medio de pago mercadopago
        /// </summary>
        /// <returns> bool: verdadero si se actualizaron los datos</returns>
        /// <remarks>
        /// Actualiza el campo medio de pago, el estado enviado por mercadolibre(pendiente o pagado), ordenid de mercadopago y preferenciaid brindada por mp
        /// </remarks>
        /// <response code="200">OK. Se actualizaron los datos</response>
        /// <response code="400">No encontrado</response>
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "EQUIPO")]
        [HttpPost("MedioPago/MercadoPago")]
        public async Task<IActionResult> ActualizarMercadopago([FromBody] PreferenciaMercadopagoDTO preferenciaMP)
        {
            try
            {
                if (preferenciaMP == null) throw new Exception("No hay preferencias de mercadopago. W69");

                bool actualizadoDatosPago = await _inscripcionService.ActualizarPagoMercadoPago(preferenciaMP);

                return Ok(actualizadoDatosPago);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        /// <summary>
        /// Permite la baja de una inscripción 
        /// </summary>
        /// <returns> bool: verdadero si se eliminan la inscripción</returns>
        /// <remarks>
        /// Elimina una inscripción segun su Id
        /// </remarks>
        /// <response code="200">OK. Se eliminó la isncripción</response>
        /// <response code="400">No encontrado</response>
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "EQUIPO")]
        [HttpDelete("Baja/{inscripcionID}")]
        public async Task<IActionResult> BajaInscripción(int inscripcionID)
        {
            try
            {
                if (inscripcionID < 1) throw new Exception("No hay inscripción disponible. W95");

                string accessTokenMercadopago = _config["AccessToeknMercadopago"];

                bool baja = await _inscripcionService.BajaInscripcion(inscripcionID,accessTokenMercadopago);

                return Ok(baja);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }




        /// <summary>
        /// Permite obtener las inscripciones de un torneo segun el rol organizador
        /// </summary>
        /// <remarks>
        /// Este endpoint permite obtener las inscripciones de un torneo segun el rol organizador
        /// </remarks>
        /// <response code="200">OK. Listado obtenido</response>
        /// <response code="400">Validaciones varias erroneas</response>
        [ProducesResponseType(typeof(List<Inscripcion>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet("Listado/InscripcionTorneo/{torneoId}")]
        public async Task<IActionResult> ObtenerInscripcionesSegunTorneo(int torneoId)
        {
            try
            {
                var inscripcionListado = await _inscripcionService.ObtenerInscripcionesSegunTorneo(torneoId);
                List<InscripcionDTO> inscripcionesDTO = _mapper.Map<List<Inscripcion>, List<InscripcionDTO>>(inscripcionListado);

                return Ok(inscripcionesDTO);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



    }
}
