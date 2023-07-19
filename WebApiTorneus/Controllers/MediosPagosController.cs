using AutoMapper;
using BDTorneus;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Negocio.DTOs;
using Negocio.Models;
using Negocio;
using static Utilidades.Util;
using WebApiTorneus.HubSignalR;
using DTOs_Compartidos.Models;

namespace WebApiTorneus.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MediosPagosController : Controller
    {
        private readonly MedioPagoService _medioPagoService;

        public MediosPagosController(MedioPagoService medioPagoService)
        {
           _medioPagoService = medioPagoService;
        }


        /// <summary>
        /// Permite la obtencion de los datos de accessToken del vendedor 
        /// </summary>
        /// <remarks>
        /// Este endpoint obtiene los datos de accessToken del vendedor para vender las inscripciones en su nombre (como intermediario)
        /// </remarks>
        /// <response code="200">OK.Devuelve AccessTokenMercadoPago</response>
        /// <response code="400">Validaciones varias no conformadas</response>
        [ProducesResponseType(typeof(IdModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "ORGANIZADOR")]
        [HttpGet("/Mercadopago/AccessTokenVenderor/{codigo}")]
        public async Task<IActionResult> PostTokenMP(string codigo)
        {
            try
            {
                if (string.IsNullOrEmpty(codigo)) BadRequest("El codigo de mercadopago está vacio");


                AccessTokenMercadoPago accToekn = await _medioPagoService.ObtenerAccessTokenVendedor(codigo);

                return Ok(accToekn);
            }
            catch (Exception ex)
            {
                    return BadRequest(ex.Message);
            }
        }






    }
}
