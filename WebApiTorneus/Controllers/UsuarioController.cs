using AutoMapper;
using BDTorneus;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Negocio;
using Negocio.DTOs;

namespace WebApiTorneus.Controllers
{
    [ApiController]
    [Route("[controller]/api")]
    public class UsuarioController : Controller
    {
        private readonly IMapper _mapper;
        private readonly UsuarioService _usuarioService;

        public UsuarioController(IMapper mapper, UsuarioService usuarioService)
        {
           _mapper = mapper;
           _usuarioService = usuarioService;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> PostLogin([FromBody] LoginDTO loginDTO)
        {
            try
            {
                loginDTO.Mail.ToLower().Trim();
                Usuario login = await _usuarioService.LoginUsuario(loginDTO);
                return Ok(_mapper.Map<UsuarioLogueado>(login));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Registro")]
        public async Task<IActionResult> PostRegistro([FromBody] RegistroDTO registroDTO)
        {
            try
            {
                var usuario = _mapper.Map<RegistroDTO, Usuario>(registroDTO);
                usuario.Mail.ToLower().Trim();
                UsuarioLogueado login = await _usuarioService.RegistroUsuario(usuario);
                return Ok(login);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }




    }
}
