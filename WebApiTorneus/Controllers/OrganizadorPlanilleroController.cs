using BDTorneus;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Negocio.DTOs;
using Negocio.Models;
using Negocio;
using static Utilidades.Util;
using DTOs_Compartidos.Models;
using WebApiTorneus.Services;

namespace WebApiTorneus.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   
    public class OrganizadorPlanilleroController : ControllerBase
    {

        private readonly AutorizacionesPlanilleros _autorizacionesPlanilleros;

        public OrganizadorPlanilleroController(AutorizacionesPlanilleros autorizacionesPlanilleros)
        {
            _autorizacionesPlanilleros = autorizacionesPlanilleros;
        }



        /// <summary>
        /// Permite agregar una autorizacion temporal que se elimina al final del torneo
        /// </summary>
        /// <remarks>
        /// Este endpoint devuelve un TorneoDTO que contiene todas las propiedades del torneo creado
        /// </remarks>
        /// <response code="200">OK.Se ha agregado</response>
        /// <response code="400">Validaciones varias no conformadas</response>
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "ORGANIZADOR")]
        [HttpPost("AgregarAutorizados")]
        public async Task<IActionResult> Agregar([FromBody] PlanilleroTorneo planillerosAut)
        {
            try
            {
                if (planillerosAut == null) BadRequest("Elemento vacio. W180");

                bool agregado = await _autorizacionesPlanilleros.AgregarPlanillleroAutorizado(planillerosAut);

                return Ok(agregado);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        /// <summary>
        /// Permite quitar una autorizacion temporal para un planillero
        /// </summary>
        /// <remarks>
        /// Este endpoint permite quitar una autorizacion temporal para un planillero
        /// </remarks>
        /// <response code="200">OK.Permisos quitados</response>
        /// <response code="400">Validaciones varias no conformadas</response>
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "ORGANIZADOR")]
        [HttpPost("QuitarAutorizado")]
        public async Task<IActionResult> QuitarAutorizacion([FromBody] PlanilleroTorneo planillerosAut)
        {
            try
            {
                if (planillerosAut == null) BadRequest("Elemento vacio. W180");

                bool quitado = await _autorizacionesPlanilleros.QuitarAutorizacion(planillerosAut);

                return Ok(quitado);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        /// <summary>
        /// Permite quitar todas las autorizaciones de un torneo especifico
        /// </summary>
        /// <remarks>
        /// Este endpoint permite quitar todas las autorizaciones de un torneo especific una vez culminado
        /// </remarks>
        /// <response code="200">OK.Autorizaciones revocadas</response>
        /// <response code="400">Validaciones varias no conformadas</response>
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "ORGANIZADOR")]
        [HttpDelete("QuitarAutorizadosTorneo/{torneoId}")]
        public async Task<IActionResult> QuitarAutorizacionesPorCierreTorneo(int torneoId)
        {
            try
            {
                if (torneoId < 1) BadRequest("Elemento vacio. W180");

                bool quitados = await _autorizacionesPlanilleros.QuitarAutorizacionesFinTorneo(torneoId);

                return Ok(quitados);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        /// <summary>
        /// Permite saber si el planillero ya está asignado a un torneo
        /// </summary>
        /// <remarks>
        /// Este endpoint permite saber si el planillero ya está asignado a un torneo especifico y saber que ya esta autorizado para una jornada especifica
        /// </remarks>
        /// <response code="200">OK.Se repite el planillero</response>
        /// <response code="400">Validaciones varias no conformadas</response>
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize]
        [HttpPost("ExistenciaPlanilleroEnTorneo")]
        public async Task<IActionResult> ExistenciaAutorizacionPlanillero([FromBody] PlanilleroTorneo planillerosAut)
        {
            try
            {
                if (planillerosAut == null) BadRequest("Elemento vacio. W180");

                bool quitados = await _autorizacionesPlanilleros.ExistenciaPlanilleroAutorizado(planillerosAut);

                return Ok(quitados);
            }
            catch (Exception ex)
            {
                    return BadRequest(ex.Message);
            }
        }


        /// <summary>
        /// Permite obtener el listado de planilleros autorizados a marcar en un torneo
        /// </summary>
        /// <remarks>
        /// Este endpoint permite obtener el listado de planilleros autorizados a marcar en un torneo
        /// </remarks>
        /// <response code="200">OK</response>
        /// <response code="400">Validaciones varias no conformadas</response>
        [ProducesResponseType(typeof(List<PlanilleroTorneo>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "ORGANIZADOR, PLANILLERO")]
        [HttpGet("ListadoPlanillerosAutorizados/{usuarioOrganizadorId}")]
        public async Task<IActionResult> AutorizacionesListado(int usuarioOrganizadorId)
        {
            try
            {
                if (usuarioOrganizadorId < 0) BadRequest("Elemento vacio. W181");

                List<PlanilleroTorneo> listadoPlanilleros = await _autorizacionesPlanilleros.ObtenerPlanillerosSegunOrganizador(usuarioOrganizadorId);

                return Ok(listadoPlanilleros);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }








    }
}
