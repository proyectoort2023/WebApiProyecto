using AutoMapper;
using BDTorneus;
using DTOs_Compartidos.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Negocio;
using Negocio.DTOs;
using static Utilidades.Util;

namespace WebApiTorneus.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FixtureController : Controller
    {
        private readonly IMapper _mapper;
        private readonly FixtureService _fixtureService;

        public FixtureController(IMapper mapper, FixtureService fixtureService)
        {
            _mapper = mapper;
            _fixtureService = fixtureService;
        }


        /// <summary>
        /// Devuelve partidos por cruzamiento de equipos
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
        public async Task<IActionResult> CrearFixtureToeno([FromBody] PartidosTorneo partidosTorneo)
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





    }
}
