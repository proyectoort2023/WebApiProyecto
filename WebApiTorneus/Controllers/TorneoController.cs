using AutoMapper;
using BDTorneus;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Negocio.DTOs;
using Negocio;
using WebApiTorneus.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Authorization;
using Negocio.Models;
using WebApiTorneus.BackgroundServices;
using Microsoft.AspNetCore.SignalR;
using WebApiTorneus.HubSignalR;
using DTOs_Compartidos.Models;

namespace WebApiTorneus.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class TorneoController : ControllerBase
    {

        private readonly IMapper _mapper;
        private readonly TorneoService _torneoService;
        private readonly IConfiguration _config;
        private const string claveRutaImagen = "Rutaimagen";
        private readonly IHubContext<TorneusHub> _hubContext;

        public TorneoController(IMapper mapper, TorneoService torneoService, IConfiguration config, IHubContext<TorneusHub> hubContext)
        {
            _mapper = mapper;
            _torneoService = torneoService;
            _config = config;
            _hubContext = hubContext;
        }


        /// <summary>
        /// Permite la creación de un torneo para el rol ORGANIZADOR
        /// </summary>
        /// <remarks>
        /// Este endpoint devuelve un TorneoDTO que contiene todas las propiedades del torneo creado
        /// </remarks>
        /// <response code="200">OK.El torneo se ha creado</response>
        /// <response code="400">Validaciones varias no conformadas</response>
        /// <response code="409">El torneo que quiere crear ya existe.</response>
        [ProducesResponseType(typeof(IdModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [Authorize(Roles = "ORGANIZADOR")]
        [HttpPost("Crear")]
        public async Task<IActionResult> PostCreacion([FromBody] TorneoCreacionDTO torneoDTO)
        {
            try
            {
                if (torneoDTO == null) BadRequest("No hay datos de torneo para registrar");

                torneoDTO.Nombre = torneoDTO.Nombre.ToUpper().Trim();
                
                Torneo torneo = _mapper.Map<Torneo>(torneoDTO);
                torneo.Usuario = new Usuario()
                {
                    Id = torneoDTO.UsuarioId
                };
                TorneoDTO torneoNuevoDTO = _mapper.Map<TorneoDTO>(await _torneoService.CrearTorneo(torneo));

                return Ok(torneoNuevoDTO);
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

        /// <summary>
        /// Permite la creación de un torneo para el rol ORGANIZADOR
        /// </summary>
        /// <remarks>
        /// Este endpoint pide un valor int para recibir una propiedad de torneoId 
        /// y devuelve un BoolModel { Bandera = bool } que contiene el un valor booleano usado como bandera para indicar
        /// si el torneo se ha suspendido por el usuario organizador.
        /// </remarks>
        /// <response code="200">OK.El torneo se ha suspendido</response>
        /// <response code="400">Validaciones varias no conformadas</response>
        [ProducesResponseType(typeof(BoolModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "ORGANIZADOR")]
        [HttpGet("Suspender/{torneoId}")]
        public async Task<IActionResult> GetSuspender(int torneoId)
        {
            try
            {
                if (torneoId < 1) BadRequest("No existe el torneo a suspender");

                BoolModel suspendido = new BoolModel(await _torneoService.SuspenderTorneo(torneoId));

                return Ok(suspendido);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        /// <summary>
        /// Permite la eliminación de un torneo para el rol ORGANIZADOR
        /// </summary>
        /// <remarks>
        /// Este endpoint pide un valor int para recibir una propiedad de torneoId 
        /// y devuelve un BoolModel { Bandera = bool } que contiene el un valor booleano usado como bandera para indicar
        /// si el torneo se ha eliminado por el usuario organizador. Solo puede ser eliminado si no hay equipos inscriptos
        /// </remarks>
        /// <response code="200">OK.El torneo se ha eliminado</response>
        /// <response code="400">Validaciones varias no conformadas</response>
        [ProducesResponseType(typeof(BoolModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "ORGANIZADOR")]
        [HttpDelete("Eliminar/{torneoId}")]
        public async Task<IActionResult> EliminarTorneo(int torneoId)
        {
            try
            {
                if (torneoId < 1) BadRequest("No existe el torneo a suspender");

                BoolModel suspendido = new( await _torneoService.EliminarTorneo(torneoId));

                return Ok(suspendido);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        /// <summary>
        /// Permite la modificación de algunos datos de un torneo para el rol ORGANIZADOR
        /// </summary>
        /// <remarks>
        /// Este endpoint recibe un objeto con las propiedades a modificar del torneo
        /// y devuelve un BoolModel { Bandera = bool } que contiene el un valor booleano usado como bandera para indicar
        /// si el torneo se ha modificado por el usuario organizador.
        /// </remarks>
        /// <response code="200">OK.El torneo se ha modificado</response>
        /// <response code="400">Validaciones varias no conformadas</response>
        [ProducesResponseType(typeof(BoolModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "ORGANIZADOR")]
        [HttpPost("Modificar")]
        public async Task<IActionResult> ModificarTorneo([FromBody] TorneoDTO torneoActualizacionDTO)
        {
            try
            {
                if (torneoActualizacionDTO == null) BadRequest("No existe el torneo para editarlo");
                torneoActualizacionDTO.Nombre = torneoActualizacionDTO.Nombre.ToUpper().Trim();

                Torneo torneo = _mapper.Map<TorneoDTO, Torneo>(torneoActualizacionDTO);

                //string urlImagenBase = _config.GetValue<string>(claveRutaImagen);
                TorneoDTO torneoModificado = _mapper.Map<Torneo, TorneoDTO>(await _torneoService.ModificarTorneo(torneo));

                return Ok(torneoModificado);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        /// <summary>
        /// Devuelve un listado de todos los torneos que creó el usuario ORGANIZADOR
        /// </summary>
        /// <remarks>
        /// Este endpoint recibe un ID de usuario y devuelve un listado de torneos que han sido creados por el usuario
        /// con rol Organizador.
        /// </remarks>
        /// <response code="200">OK. Se devuelve la lista de torneos</response>
        /// <response code="400">Validaciones varias no conformadas</response>
        [ProducesResponseType(typeof(List<Torneo>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "ORGANIZADOR")]
        [HttpGet("MisTorneos/{usuarioId}")]
        public async Task<IActionResult> GetMisTorneosOrganizador(int usuarioId)
        {
            try
            {
                var listaTorneos = await _torneoService.MisTorneosOrganizador(usuarioId);

                return Ok(listaTorneos);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        /// <summary>
        /// Devuelve un listado de todos los torneos vigentes para inscribirse o están siendo jugados
        /// </summary>
        /// <remarks>
        /// Este endpoint devuelve un listado de torneos vigentes a la fecha de hoy. 
        /// Pueden solicitarlos todos los usuarios
        /// </remarks>
        /// <response code="200">OK. Se devuelve la lista de torneos</response>
        /// <response code="400">Validaciones varias no conformadas</response>
        [ProducesResponseType(typeof(List<Torneo>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize]
        [HttpGet("TorneosVigentes")]
        public async Task<IActionResult> GetTorneos()
        {
            try
            {
                var listaTorneosVigentes = await _torneoService.ObtenerTorneosVigentes();

                return Ok(listaTorneosVigentes);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        /// <summary>
        /// Habilita la inscripción de los equipos a un torneo
        /// </summary>
        /// <remarks>
        /// Este endpoint habilita por parte del organizador la inscripción de los equipos al torneo
        /// </remarks>
        /// <response code="200">OK. Inscripciones abiertas</response>
        /// <response code="400">No se pudo habilitar</response>
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize]
        [HttpGet("HabilitarInscripcion/{idTorneo}")]
        public async Task<IActionResult> GetHabilitarInscripcion(int idTorneo)
        {
            try
            {
                bool habilitarTorneo = await _torneoService.AbrirInscripciones(idTorneo);

                if (!habilitarTorneo) return BadRequest("No existe el torneo. Cod 23");
                return Ok(habilitarTorneo);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        /// <summary>
        /// Cierra la inscripción de los equipos a un torneo
        /// </summary>
        /// <remarks>
        /// Este endpoint cierra por parte del organizador la inscripción de los equipos al torneo
        /// </remarks>
        /// <response code="200">OK. Inscripción cerrada</response>
        /// <response code="400">No se pudo cerrar</response>
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize]
        [HttpGet("CerrarInscripcion/{idTorneo}")]
        public async Task<IActionResult> GetCerrarInscripcion(int idTorneo)
        {
            try
            {
                bool cerrarTorneo = await _torneoService.CerrarInscripciones(idTorneo);

                if (!cerrarTorneo) return BadRequest("No existe el torneo. Cod 24");
                return Ok(cerrarTorneo);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Devuelve las inscripciones de un determinado torneo
        /// </summary>
        /// <remarks>
        /// Este endpoint devuelve las inscripciones de un determinado torneo
        /// </remarks>
        /// <response code="200">OK.Listado de inscripciones obtenidos</response>
        /// <response code="400">No se obtener el listado</response>
        [ProducesResponseType(typeof(List<InscripcionDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "ORGANIZADOR")]
        [HttpGet("Inscripciones/{torneoId}")]
        public async Task<IActionResult> GetInscripcionesTorneo(int torneoId)
        {
            try
            {
                List<InscripcionDTO> inscripcionesTorneo = _mapper.Map<List<Inscripcion>, List<InscripcionDTO>>(await _torneoService.ObtenerInscripcionesTorneo(torneoId));

                if (inscripcionesTorneo == null) return BadRequest("No existe inscripciones. Cod 99");
                return Ok(inscripcionesTorneo);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


       



    }
}
