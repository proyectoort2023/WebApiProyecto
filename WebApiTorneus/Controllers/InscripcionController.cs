using AutoMapper;
using BDTorneus;
using DTOs_Compartidos.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Negocio;
using Negocio.DTOs;
using Negocio.Models;
using System.Collections.Generic;
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
        public InscripcionController(IMapper mapper, UsuarioService usuarioService, InscripcionService inscripcionService)
        {
            _mapper = mapper;
            _inscripcionService = inscripcionService;
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
                Inscripcion inscripcion = _mapper.Map<InscripcionDTO,Inscripcion>(inscripcionDTO);

                Inscripcion inscripcionNueva = await _inscripcionService.AgregarNuevaInscripcion(inscripcion);
                InscripcionDTO inscripcionNuevaDTO = _mapper.Map<Inscripcion, InscripcionDTO>(inscripcionNueva);

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
        [ProducesResponseType(typeof(TokenModel), StatusCodes.Status200OK)]
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
        public async Task<IActionResult> WebHooksMercadoPago([FromBody] WebHook webHook)
        {
            try
            {
            

                return Ok(webHook);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }




    }
}
