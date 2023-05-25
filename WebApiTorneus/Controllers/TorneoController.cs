using AutoMapper;
using BDTorneus;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Negocio.DTOs;
using Negocio;
using WebApiTorneus.Services;
using Microsoft.AspNetCore.Http.HttpResults;

namespace WebApiTorneus.Controllers
{
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
    }
}
