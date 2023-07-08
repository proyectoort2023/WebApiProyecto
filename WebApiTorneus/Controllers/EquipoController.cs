using AutoMapper;
using BDTorneus;
using DTOs_Compartidos.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Negocio;
using Negocio.DTOs;
using Negocio.Models;
using WebApiTorneus.HubSignalR;
using static Utilidades.Util;

namespace WebApiTorneus.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EquipoController : Controller
    {
            private readonly IMapper _mapper;
            private readonly EquipoService _equipoService;

            public EquipoController(IMapper mapper, EquipoService equipoService)
            {
                _mapper = mapper;
                _equipoService = equipoService;
            }


            /// <summary>
            /// Permite la obtención de un listado de equipos que administra el rol administrador de equipos 
            /// </summary>
            /// <remarks>
            /// Este endpoint pide un valor int que contiene el id del usuario que pide el listado
            /// y se filtra por los equipos que administra el usuario
            /// </remarks>
            /// <response code="200">OK. Listado de equipos obtenido</response>
            /// <response code="400">Validaciones varias no conformadas</response>
            [ProducesResponseType(typeof(List<EquipoDTO>), StatusCodes.Status200OK)]
            [ProducesResponseType(StatusCodes.Status400BadRequest)]
            [Authorize(Roles = "EQUIPO")]
            [HttpGet("ObtenerEquipos/{usuarioId}")]
            public async Task<IActionResult> GetSuspender(int usuarioId)
            {
                try
                {
                    if (usuarioId < 1) BadRequest("No existe el administrador de equipos");

                    List<Equipo> listado = await _equipoService.ObtenerEquiposDeAdministrador(usuarioId);

                    List<EquipoDTO> equiposDTO = _mapper.Map<List<Equipo>, List<EquipoDTO>>(listado);

                    return Ok(equiposDTO);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }





    }
}
