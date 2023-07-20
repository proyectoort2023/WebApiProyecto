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
    public class MediosPagosController : ControllerBase
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
        [HttpPost("Mercadopago/AccessTokenVendedor/")]
        public async Task<IActionResult> PostTokenMercadopago([FromBody] MpAuthVendedor mpAuthVendedor)
        {
            try
            {
                if (mpAuthVendedor == null ) BadRequest("El codigo de mercadopago está vacio");


                bool cuentaIntegradaVendedorMercadoPago = await _medioPagoService.ImplementarMercadoPagoVendedor(mpAuthVendedor.Codigo, mpAuthVendedor.UsuarioId);

                return Ok(cuentaIntegradaVendedorMercadoPago);
            }
            catch (Exception ex)
            {
                    return BadRequest(ex.Message);
            }
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
        [Authorize(Roles = "EQUIPO")]
        [HttpGet("Mercadopago/AccessTokenVendedor/{usuarioId}")]
        public async Task<IActionResult> GetTokenMercadopago(int usuarioId)
        {
            try
            {
                //if (mpAuthVendedor == null) BadRequest("El codigo de mercadopago está vacio");


                string tokenVendedorMercadoPago = await _medioPagoService.ObtenerAccesTokenVendedor(usuarioId);

                return Ok(tokenVendedorMercadoPago);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }




    }
}
