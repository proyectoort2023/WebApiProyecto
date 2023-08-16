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
    [Authorize(Roles = "ORGANIZADOR")]
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
        /// <response code="200">OK.El torneo se ha creado</response>
        /// <response code="400">Validaciones varias no conformadas</response>
        [ProducesResponseType(typeof(IdModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
                if (ex.Message == "DUPLICADO")
                {
                    return Conflict("El torneo ya existe.");
                }
                else
                {
                    return BadRequest(ex.Message);
                }
            }
        }
    }
}
