using AutoMapper;
using BDTorneus;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;
using Negocio;
using Negocio.DTOs;
using Negocio.Models;
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
        /// Permite a un usuario con rol ORGANIZADOR o EQUIPO loguearse en la app
        /// </summary>
        /// <returns> Devuelve un string que conetiene un token JWT</returns>
        /// <remarks>
        /// Este endpoint devuelve un string que conetiene un token JWT de tipo
        /// { Id = int, Mail = string, Rol = "ESPECTADOR" o "EQUIPO" o "ESPECTADOR" o "PLANILLERO", Token = string }
        /// </remarks>
        /// <response code="200">OK. El usuario se encontró correctamente. Se devuelve un token JWT</response>
        /// <response code="400">No encontrado</response>
        [ProducesResponseType(typeof(TokenModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost("Login")]
        public async Task<IActionResult> PostLogin([FromBody] LoginDTO loginDTO)
        {
            try
            {
                loginDTO.Mail.ToLower().Trim(); 
                Usuario login = await _usuarioService.LoginUsuario(loginDTO);
                var usuarioLogueado = _mapper.Map<UsuarioLogueado>(login);

                var secretkey = _config["Jwt:SecretKey"];

                var token = new TokenModel()
                {
                    Token = GeneradorToken.CrearToken(usuarioLogueado, _config)
                };

                return Ok(token);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        /// <summary>
        /// Permite el registro de un usuario para rol ORGANIZADOR o EQUIPO
        /// </summary>
        /// <remarks>
        /// Este endpoint devuelve token de tipo JWT con el usuario registrado
        /// { Id = int, Mail = string, Rol = "ESPECTADOR" o "EQUIPO" o "ESPECTADOR" o "PLANILLERO", Token = string }
        /// </remarks>
        /// <response code="200">OK. El usuario se encontró correctamente. Se devuelve un token JWT</response>
        /// <response code="400">No encontrado</response>
        /// <response code="409">El usuario con el ID especificado ya existe.</response>
        [ProducesResponseType(typeof(TokenModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [HttpPost("Registro")]
        public async Task<IActionResult> PostRegistro([FromBody] RegistroDTO registroDTO)
        {
            try
            {
                var usuario = _mapper.Map<RegistroDTO, Usuario>(registroDTO);
                usuario.Mail.ToLower().Trim();
                Usuario registro = await _usuarioService.RegistroUsuario(usuario);
                var registradoRealizado = _mapper.Map<UsuarioLogueado>(registro);

                var token = new TokenModel()
                {
                    Token = GeneradorToken.CrearToken(registradoRealizado, _config)
                };

                return Ok(token);
            }
            catch (Exception ex)
            {
                if (ex.Message == "DUPLICADO")
                {
                    return Conflict("El usuario ya existe");
                }
                else
                {
                return BadRequest(ex.Message);

                }
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
        [HttpGet("LoginEspectador")]
        public async Task<IActionResult> GetLoginEspectador()
        {
            try
            {
                Usuario login = await _usuarioService.LoginUsuarioEspectador();
                var usuarioLogueado = _mapper.Map<UsuarioLogueado>(login);

                var secretkey = _config["Jwt:SecretKey"];

                var token = new TokenModel()
                {
                    Token = GeneradorToken.CrearToken(usuarioLogueado, _config)
                };

                return Ok(token);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }




    }
}
