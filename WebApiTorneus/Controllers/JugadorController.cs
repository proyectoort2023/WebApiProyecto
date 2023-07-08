using AutoMapper;
using BDTorneus;
using DTOs_Compartidos.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Negocio;
using Negocio.DTOs;

namespace WebApiTorneus.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JugadorController : Controller
    {
        private readonly IMapper _mapper;
        private readonly JugadorService _jugadorService;

        public JugadorController(IMapper mapper, EquipoService equipoService, JugadorService jugadorService)
        {
            _mapper = mapper;
            _jugadorService = jugadorService;
        }



        /// <summary>
        /// Permite la obtención de un listado de todos los jugadores registrados en el sistema 
        /// </summary>
        /// <remarks>
        /// Este endpoint devuelve todos los jugadores registrados en el sistema 
        /// </remarks>
        /// <response code="200">OK. Listado de equipos obtenido</response>
        /// <response code="400">Validaciones varias no conformadas</response>
        [ProducesResponseType(typeof(List<JugadorDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "EQUIPO")]
        [HttpGet("ObtenerJugadores")]
        public async Task<IActionResult> GetJugadores()
        {
            try
            {

                List<Jugador> listado = await _jugadorService.ObtenerTodosJugadores();

                List<JugadorDTO> jugadoresDTO = _mapper.Map<List<Jugador>, List<JugadorDTO>>(listado);

                return Ok(jugadoresDTO);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Permite el registro de un nuevo jugador
        /// </summary>
        /// <remarks>
        /// Este endpoint registra si no está duplicado un nuevo jugador
        /// </remarks>
        /// <response code="200">OK. Jugador creado </response>
        /// <response code="400">Validaciones varias no conformadas</response>
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "EQUIPO")]
        [HttpPost("Crear")]
        public async Task<IActionResult> CrearJugador([FromBody] JugadorDTO jugadorDTO)
        {
            try
            {
                Jugador jugador = _mapper.Map<JugadorDTO, Jugador>(jugadorDTO);
                int idJugadorNuevo = await _jugadorService.RegistrarJugador(jugador);
                return Ok(idJugadorNuevo);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }




    }
}
