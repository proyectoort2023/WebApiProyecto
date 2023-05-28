using AutoMapper;
using BDTorneus;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Negocio;
using Negocio.DTOs;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
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

        /// <summary>
        /// Obtiene un objeto de ejemplo por su ID.
        /// </summary>
        /// <param name="id">ID del objeto de ejemplo.</param>
        /// <returns>Objeto de ejemplo encontrado.</returns>
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[SwaggerOperation("ObtenerEjemploPorId", "Obtiene un objeto de ejemplo por su ID.")]
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
                Usuario registro = await _usuarioService.RegistroUsuario(usuario);
                var registradoRealizado = _mapper.Map<UsuarioLogueado>(registro);

                var token = GeneradorToken.CrearToken(registradoRealizado, _config);

                return Ok(token);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("LoginEspectador")]
        public async Task<IActionResult> GetLoginEspectador()
        {
            try
            {
                Usuario login = await _usuarioService.LoginUsuarioEspectador();
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




    }
}
