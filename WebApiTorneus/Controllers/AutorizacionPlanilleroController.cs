using AutoMapper;
using BDTorneus;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Negocio.DTOs;
using Negocio;
using static Utilidades.Util;
using DTOs_Compartidos.DTOs;

namespace WebApiTorneus.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutorizacionPlanilleroController : ControllerBase
    {
        private readonly AutorizacionPlanilleroService _autorizacionPlanilleroService;

        public AutorizacionPlanilleroController(AutorizacionPlanilleroService autorizacionPlanilleroService)
        {
            _autorizacionPlanilleroService = autorizacionPlanilleroService;
        }


        /// <summary>
        /// Permite el registro de una notificación
        /// </summary>
        /// <remarks>
        /// Este endpoint permite el registro de una notificación
        /// </remarks>
        /// <response code="200">OK.</response>
        /// <response code="400">Validaciones varias no conformadas</response>
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "ORGANIZADOR")]
        [HttpPost("Agregar")]
        public async Task<IActionResult> Agregar([FromBody] AutorizacionPlanilleroDTO autPlanilleroDTO)
        {
            try
            {
                if (autPlanilleroDTO == null) BadRequest("No existe autorizacion de planillero");

                int autPlanilleroId = await _autorizacionPlanilleroService.AgregarPlanillleroAutorizado(autPlanilleroDTO);

                return Ok(autPlanilleroId);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        /// <summary>
        /// Permite obtener el listado de todas las autorizaciones de un planillero de todos los organizadores que le dieron acceso a la marcación de su torneo
        /// </summary>
        /// <remarks>
        /// Este endpoint permite obtener el listado de todas las autorizaciones de un planillero de todos los organizadores que le dieron acceso a la marcación de su torneo
        /// </remarks>
        /// <response code="200">OK.</response>
        /// <response code="400">Validaciones varias no conformadas</response>
        [ProducesResponseType(typeof(List<AutorizacionPlanilleroDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "PLANILLERO")]
        [HttpGet("ObtenerAutorizacionesPlanillero/{planilleroId}")]
        public async Task<IActionResult> ObtenerListadoAutorizacionesPlanilleros(int planilleroId)
        {
            try
            {
                if (planilleroId < 1) BadRequest("No existe autorizacion de planillero");
                List<AutorizacionPlanilleroDTO> autPlanillerosDTO = new();

                List<AutorizacionPlanillero> autPlanilleros = await _autorizacionPlanilleroService.ObtenerAutorizacionesPlanillero(planilleroId);

                if (autPlanilleros.Count > 0)
                {
                    autPlanillerosDTO = autPlanilleros.Select(s => new AutorizacionPlanilleroDTO()
                    {
                        Id = s.Id,
                        UsuarioIdOrganizador = s.UsuarioOrganizador.Id,
                        UsuarioIdPlanillero = s.UsuarioPlanillero.Id,
                        TorneoId = s.Torneo.Id,
                        EmailPlanillero = s.UsuarioPlanillero.Mail,
                        NombrePlanillero = s.UsuarioPlanillero.Nombre,
                        FechaTorneo = s.Torneo.Fecha,
                        NombreTorneo = s.Torneo.Nombre,

                    }).ToList();

                }

                return Ok(autPlanillerosDTO);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Permite obtener el listado de todas las autorizaciones que el organizador le brindó a un determinado planillero
        /// </summary>
        /// <remarks>
        /// Este endpoint permite obtener el listado de todas las autorizaciones que el organizador le brindó a un determinado planillero
        /// </remarks>
        /// <response code="200">OK.</response>
        /// <response code="400">Validaciones varias no conformadas</response>
        [ProducesResponseType(typeof(List<AutorizacionPlanilleroDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "ORGANIZADOR")]
        [HttpGet("ObtenerAutorizacionesOrganizador/{organizadorId}")]
        public async Task<IActionResult> ObtenerListadoAutorizacionesOrganizador(int organizadorId)
        {
            try
            {
                if (organizadorId < 1) BadRequest("No existe autorizacion de planillero");
                List<AutorizacionPlanilleroDTO> autPlanillerosDTO = new();

                List<AutorizacionPlanillero> autPlanilleros = await _autorizacionPlanilleroService.ObtenerAutorizacionesPlanilleroOrganizador(organizadorId);

                if (autPlanilleros.Count > 0)
                {
                    autPlanillerosDTO = autPlanilleros.Select(s => new AutorizacionPlanilleroDTO()
                    {
                        Id = s.Id,
                        UsuarioIdOrganizador = s.UsuarioOrganizador.Id,
                        UsuarioIdPlanillero = s.UsuarioPlanillero.Id,
                        TorneoId = s.Torneo.Id,
                        EmailPlanillero = s.UsuarioPlanillero.Mail,
                        NombrePlanillero = s.UsuarioPlanillero.Nombre,
                        FechaTorneo = s.Torneo.Fecha,
                        NombreTorneo = s.Torneo.Nombre,

                    }).ToList();

                }

                return Ok(autPlanillerosDTO);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        /// <summary>
        /// Permite el borrado de una autorizacion al marcador a determinado planillero a cierto torneo
        /// </summary>
        /// <remarks>
        /// Este endpoint permite el borrado de una autorizacion al marcador a determinado planillero a cierto torneo
        /// </remarks>
        /// <response code="200">OK.</response>
        /// <response code="400">Validaciones varias no conformadas</response>
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "ORGANIZADOR")]
        [HttpPost("EliminarAutorizacion")]
        public async Task<IActionResult> QuitarAutorizacion([FromBody] AutorizacionPlanilleroDTO autPlanilleroDTO)
        {
            try
            {
                if (autPlanilleroDTO == null) BadRequest("No existe autorizacion de planillero");

                bool borrado = await _autorizacionPlanilleroService.QuitarAutorizacion(autPlanilleroDTO);

                return Ok(borrado);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        /// <summary>
        /// Permite el borrado de todas las autorizaciones a marcador de todos los usuarios planilleros involucrados en un torneo al finalizar el mismo
        /// </summary>
        /// <remarks>
        /// Este endpoint permite el borrado de todas las autorizaciones a marcador de todos los usuarios planilleros involucrados en un torneo al finalizar el mismo
        /// </remarks>
        /// <response code="200">OK.</response>
        /// <response code="400">Validaciones varias no conformadas</response>
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "PLANILLERO")]
        [HttpDelete("EliminarAutorizacionTorneo/{torneoId}")]
        public async Task<IActionResult> QuitarAutorizacionesTorneo(int torneoId)
        {
            try
            {
                if (torneoId < 1) BadRequest("No existe autorizacion de planillero");

                bool borrado = await _autorizacionPlanilleroService.QuitarAutorizacionesFinTorneo(torneoId);

                return Ok(borrado);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        /// <summary>
        /// Permite obtener del usuario si es de rol planillero para autorizaciones de marcaciones por parte del organizador
        /// </summary>
        /// <remarks>
        /// Este endpoint permite obtener del usuario si es de rol planillero para autorizaciones de marcaciones por parte del organizador
        /// </remarks>
        /// <response code="200">OK.</response>
        /// <response code="400">Validaciones varias no conformadas</response>
        [ProducesResponseType(typeof(AutorizacionPlanilleroDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "ORGANIZADOR")]
        [HttpGet("ObtenerUsuarioMail/{mail}")]
        public async Task<IActionResult> ObtenerUsuarioAAutorizar(string mail)
        {
            try
            {
                if (string.IsNullOrEmpty(mail)) BadRequest("No existe autorizacion de planillero");

                AutorizacionPlanilleroDTO planilleroEncontrado = await _autorizacionPlanilleroService.ObtenerUsuarioParaAutorizar(mail);

                return Ok(planilleroEncontrado);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



    }
}
