using AutoMapper;
using BDTorneus;
using DTOs_Compartidos.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Negocio;
using Negocio.DTOs;
using WebApiTorneus.Services;
using static Utilidades.Util;

namespace WebApiTorneus.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FixtureController : Controller
    {
        private readonly IMapper _mapper;
        private readonly FixtureService _fixtureService;
        private readonly TorneoService _torneoService;

        private readonly FixtureTiempoReal _fixtureTiempoReal;

        public FixtureController(IMapper mapper, FixtureService fixtureService, FixtureTiempoReal fixtureTiempoReal, TorneoService torneoService)
        {
            _mapper = mapper;
            _fixtureService = fixtureService;
            _fixtureTiempoReal = fixtureTiempoReal;
            _torneoService = torneoService;
        }


        /// <summary>
        /// Crea el fixture segun los partidos del cruzamiento de equipos enviados por el usuario organizador
        /// </summary>
        /// <remarks>
        /// Este endpoint devuelve partidos por cruzamiento de equipos
        /// </remarks>
        /// <response code="200">OK.Listado de partidos obtenidos</response>
        /// <response code="400">No se obtener el listado</response>
        [ProducesResponseType(typeof(List<PartidoDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "ORGANIZADOR")]
        [HttpPost("Crear")]
        public async Task<IActionResult> CrearFixtureTorneo([FromBody] PartidosTorneo partidosTorneo)
        {
            try
            {
                List<Partido> partidos = _mapper.Map<List<PartidoDTO>, List<Partido>>(partidosTorneo.Fixture);


                List<PartidoDTO> partidosCreados = _mapper.Map<List<Partido>, List<PartidoDTO>>(await _fixtureService.CrearFixture(partidosTorneo.TorneoId, partidos));

                if (partidosCreados == null) return BadRequest("No se pudo crear el fixture. Cod 151");

                return Ok(partidosCreados);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        /// <summary>
        /// Devuelve partidos por cruzamiento de equipos desde la base de datos
        /// </summary>
        /// <remarks>
        /// Este endpoint devuelve partidos por cruzamiento de equipos desde la base de datos
        /// </remarks>
        /// <response code="200">OK.Listado de partidos obtenidos</response>
        /// <response code="400">No se obtener el listado</response>
        [ProducesResponseType(typeof(List<PartidoDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize]
        [HttpGet("ListadoPartidos/{torneoId}")]
        public async Task<IActionResult> ListadoPartidos(int torneoId)
        {
            try
            {
                List<PartidoDTO> partidos = new();

                partidos = _fixtureTiempoReal.ObtenerFixtureTorneo(torneoId);

                if (partidos.Count == 0)
                {
                    partidos = _mapper.Map<List<Partido>, List<PartidoDTO>>(await _fixtureService.ObtenerPartidosTorneo(torneoId));
                    bool torneoCerrado = await _torneoService.TorneoEstaCerrado(torneoId);
                    if (!torneoCerrado)
                    {
                        await _fixtureTiempoReal.CargarFixture(partidos);
                    }
                }

                if (partidos == null) return BadRequest("No se pudo obtener el fixture. Cod 159");

                return Ok(partidos);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        /// <summary>
        /// Actualiza los datos de un partido al finalizar el mismo
        /// </summary>
        /// <remarks>
        /// Este endpoint actualiza los datos de un partido al finalizar el mismo
        /// </remarks>
        /// <response code="200">OK.Partido actualizado</response>
        /// <response code="400">No se obtener el listado</response>
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "PLANILLERO")]
        [HttpPost("ActualizarPartido")]
        public async Task<IActionResult> Actualizar([FromBody] PartidoDTO partidoDTO)
        {
            try
            {
                if (partidoDTO == null) throw new Exception("No hay partido a actualizar. W203");

                Partido partido = _mapper.Map<PartidoDTO, Partido>(partidoDTO);

                bool partidoActualizado = await _fixtureService.ActualizarPartido(partido);

                return Ok(partidoActualizado);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }
}
