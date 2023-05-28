using AutoMapper;
using BDTorneus;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Negocio.DTOs;
using Negocio;
using WebApiTorneus.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Authorization;

namespace WebApiTorneus.Controllers
{
    [Authorize(Roles = "ORGANIZADOR")]
    [Route("api/[controller]")]
    [ApiController]
    public class TorneoController : ControllerBase
    {

        private readonly IMapper _mapper;
        private readonly TorneoService _torneoService;
        private readonly IConfiguration _config;
        private const string claveRutaImagen = "Rutaimagen";

        public TorneoController(IMapper mapper, TorneoService torneoService, IConfiguration config)
        {
            _mapper = mapper;
            _torneoService = torneoService;
            _config = config;
        }

       
        [HttpPost("Crear")]
        public async Task<IActionResult> PostCreacion([FromBody] TorneoCreacionDTO torneoDTO)
        {
            try
            {
                if (torneoDTO == null) BadRequest("No hay datos de torneo para registrar");

                torneoDTO.Nombre.ToUpper().Trim();
                
                Torneo torneo = _mapper.Map<Torneo>(torneoDTO);
                string urlImagenBase = _config.GetValue<string>(claveRutaImagen);
                int idTorneo = await _torneoService.CrearTorneo(torneo, urlImagenBase);

                return Ok(idTorneo);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
       
        [HttpGet("Suspender/{id}")]
        public async Task<IActionResult> GetSuspender(int torneoId)
        {
            try
            {
                if (torneoId <1) BadRequest("No existe el torneo a suspender");

                bool suspendido =  await _torneoService.SuspenderTorneo(torneoId);

                return Ok(suspendido);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("Eliminar/{id}")]
        public async Task<IActionResult> EliminarTorneo(int torneoId)
        {
            try
            {
                if (torneoId < 1) BadRequest("No existe el torneo a suspender");

                bool suspendido = await _torneoService.EliminarTorneo(torneoId);

                return Ok(suspendido);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Modificar")]
        public async Task<IActionResult> ModificarTorneo([FromBody] TorneoActualizacionDTO torneoActualizacionDTO)
        {
            try
            {
                if (torneoActualizacionDTO == null) BadRequest("No existe el torneo para editarlo");

                Torneo torneo = _mapper.Map<TorneoActualizacionDTO, Torneo>(torneoActualizacionDTO);

                string urlImagenBase = _config.GetValue<string>(claveRutaImagen);
                bool torneoModificado = await _torneoService.ModificarTorneo(torneo, urlImagenBase);

                return Ok(torneoModificado);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("torneos/mistorneos/{id}")]
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

        [HttpGet("torneos/vigentes")]
        public async Task<IActionResult> GetTorneos(int usuarioId)
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




    }
}
