using AutoMapper;
using BDTorneus;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Negocio;
using Negocio.DTOs;
using WebApiTorneus.Services;

namespace WebApiTorneus.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : Controller
    {
        private readonly IMapper _mapper;
        private readonly UsuarioService _usuarioService;
        private readonly IConfiguration _config;
        public UsuarioController(IMapper mapper, UsuarioService usuarioService, IConfiguration config)
        {
           _mapper = mapper;
           _usuarioService = usuarioService;
           _config = config;
        }

             
        [HttpPost("Login")]
        public async Task<IActionResult> PostLogin([FromBody] LoginDTO loginDTO)
        {
            try
            {
                loginDTO.Mail.ToLower().Trim(); 
                Usuario login = await _usuarioService.LoginUsuario(loginDTO);
                var usuarioLogueado = _mapper.Map<UsuarioLogueado>(login);

                var secretkey = _config["Jwt:SecretKey"];

                var token = GeneradorToken.CrearToken(usuarioLogueado, _config);

                return Ok(token);
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
                UsuarioLogueado registrado = await _usuarioService.RegistroUsuario(usuario);

                var token = GeneradorToken.CrearToken(registrado, _config);

                return Ok(token);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


            


    }
}
