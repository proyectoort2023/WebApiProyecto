using AutoMapper;
using BDTorneus;
using Microsoft.AspNetCore.Mvc;
using Negocio;
using Negocio.DTOs;
using Negocio.Models;
using WebApiTorneus.Services;

namespace WebApiTorneus.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InscripcionController : Controller
    {
        private readonly IMapper _mapper;
        private readonly InscripcionService _inscripcionService;
        public InscripcionController(IMapper mapper, UsuarioService usuarioService, InscripcionService inscripcionService)
        {
            _mapper = mapper;
            _inscripcionService = inscripcionService;
        }


        /// <summary>
        /// Permite a un usuario de equipo agregar una inscripcion de un equipo a un torneo
        /// </summary>
        /// <returns> Agreaga una nueva inscripción a un torneo</returns>
        /// <remarks>
        /// Agreaga una nueva inscripción a un torneo
        /// </remarks>
        /// <response code="200">OK. se ha inscripto el equipo al torneos</response>
        /// <response code="400">No encontrado</response>
        [ProducesResponseType(typeof(TokenModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost("Agregar")]
        public async Task<IActionResult> AgregarInscripcionNueva([FromBody] InscripcionDTO inscripcionDTO)
        {
            try
            {
                Inscripcion inscripcion = _mapper.Map<InscripcionDTO,Inscripcion>(inscripcionDTO);

                Inscripcion inscripcionNueva = await _inscripcionService.AgregarNuevaInscripcion(inscripcion);
                InscripcionDTO inscripcionNuevaDTO = _mapper.Map<Inscripcion, InscripcionDTO>(inscripcionNueva);

                return Ok(inscripcionNuevaDTO);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        /// <summary>
        /// Permite el login de un usuario con rol Espectador
        /// </summary>
        /// <remarks>
        /// Este endpoint devuelve token de tipo JWT con el usuario logueado
        /// { Id = int, Mail = string, Rol = "ESPECTADOR" o "EQUIPO" o "ESPECTADOR" o "PLANILLERO", Token = string }
        /// </remarks>
        /// <response code="200">OK. El usuario se encontró correctamente. Se devuelve un token JWT</response>
        /// <response code="400">Validaciones varias erroneas</response>
        [ProducesResponseType(typeof(TokenModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet("Listado/{usuarioId}")]
        public async Task<IActionResult> GetLoginEspectador(int usuarioId)
        {
            try
            {
                var inscripcionListado = await _inscripcionService.ObtenerInscripcionesSegunUsuario(usuarioId);
                List<InscripcionDTO> inscripcionesDTO = _mapper.Map<List<InscripcionDTO>, List<Inscripcion>>(inscripcionListado);

                return Ok(inscripcionesDTO);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }








    }
}
