using AutoMapper;
using BDTorneus;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Negocio.DTOs;
using Negocio;
using WebApiTorneus.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Authorization;
using Utilidades;

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

                int idTorneo = await _torneoService.CrearTorneo(torneo);

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




    }
}
