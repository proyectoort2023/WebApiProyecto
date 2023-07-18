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



        /// <summary>
        /// Permite la creación de un equipo para el rol EQUIPO
        /// </summary>
        /// <remarks>
        /// Este endpoint permite crear al administrador de equipos un equipo nuevo
        /// </remarks>
        /// <response code="200">OK.El equipo se ha creado</response>
        /// <response code="400">Validaciones varias no conformadas</response>
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "EQUIPO")]
        [HttpPost("Crear")]
        public async Task<IActionResult> PostCreacion([FromBody] EquipoDTO equipoDTO)
        {
            try
            {
                if (equipoDTO == null) BadRequest("No hay datos de equipo para registrar. W51");

                equipoDTO.Nombre = equipoDTO.Nombre.ToUpper().Trim();
                equipoDTO.Abreviatura = equipoDTO.Abreviatura.ToUpper().Trim();
                equipoDTO.Capitan = string.IsNullOrEmpty(equipoDTO.Capitan) ? "" : equipoDTO.Capitan;

                Equipo equipo = _mapper.Map<Equipo>(equipoDTO);

                int equipoNuevoId = await _equipoService.CrearEquipoNuevo(equipo);

                return Ok(equipoNuevoId);
            }
            catch (Exception ex)
            {
              return BadRequest(ex.Message);
            }
        }


        /// <summary>
        /// Permite la modificación de la lsita de jugadires de un equipo
        /// </summary>
        /// <remarks>
        /// Este endpoint permite modificar la lista de jugadores de un equipo por el administrador de equipos
        /// </remarks>
        /// <response code="200">OK.El equipo se ha modificado</response>
        /// <response code="400">Validaciones varias no conformadas</response>
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "EQUIPO")]
        [HttpPost("Modificar")]
        public async Task<IActionResult> PostModificacion([FromBody] EquipoDTO equipoDTO)
        {
            try
            {
                if (equipoDTO == null) BadRequest("No hay datos de equipo para registrar. W51");

                equipoDTO.Capitan = string.IsNullOrEmpty(equipoDTO.Capitan) ? "" : equipoDTO.Capitan;

                Equipo equipo = _mapper.Map<Equipo>(equipoDTO);

                EquipoDTO equipoModificado = _mapper.Map<Equipo,EquipoDTO>(await _equipoService.ModificarDatosEquipo(equipo));

                return Ok(equipoModificado);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }




    }
}
