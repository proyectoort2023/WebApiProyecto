using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Negocio;
using Negocio.Models;
using static Utilidades.Util;

namespace WebApiTorneus.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ImagenesController : ControllerBase
    {
        private readonly ImagenService _imagenService;
        private readonly string _conexionStorage;
        private readonly string _storageNameAzure;
        private readonly IConfiguration _config;

        public ImagenesController(ImagenService imagenService, IConfiguration config)
        {
            _imagenService = imagenService;
            _config = config;
            _conexionStorage = _config.GetValue<string>("CadenaConexionAzureStorage");
            _storageNameAzure = _config.GetValue<string>("AzureStorageName");
        }


        /// <summary>
        /// Permite a un usuario con rol ORGANIZADOR o EQUIPO subir una imagen para el banner o logo
        /// </summary>
        /// <remarks>
        /// Este endpoint devuelve un string que contiene el nombre de la imagen que figura en Azure Storage. 
        /// </remarks>
        /// <response code="200">OK. El archivo de imagen se ha subido</response>
        /// <response code="400">No encontrado</response>
        [ProducesResponseType(typeof(TokenModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost("SubirImagen")]
        public async Task<IActionResult> SubirImagen([FromForm] IFormFile imagenModel)
        {
            try
            {
                string nombreArchivo = await _imagenService.SubirImagen(_conexionStorage, _storageNameAzure, imagenModel);
                StringModel archivo = new(nombreArchivo);
                if (nombreArchivo == null) return NoContent();

                return Ok(archivo);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }


        /// <summary>
        /// Permite a un usuario con rol ORGANIZADOR o EQUIPO actualizar la imagen de un banner o logo
        /// </summary>
        /// <remarks>
        /// Este endpoint devuelve un string que contiene el nombre de la imagen que figura en Azure Storage. 
        /// </remarks>
        /// <response code="200">OK. El archivo de imagen se ha actualizado</response>
        /// <response code="400">No encontrado</response>
        [ProducesResponseType(typeof(TokenModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost("ActualizarImagen")]
        public async Task<IActionResult> ActualizarImagen([FromForm] IFormFile imagenModel)
        {
            try
            {
                string nombreArchivo = await _imagenService.ActualizarImagen(_conexionStorage, _storageNameAzure, imagenModel, imagenModel.FileName);
                
                if (nombreArchivo == null) return NoContent();

                StringModel nombrearchivoNuevo = new(nombreArchivo);
                return Ok(nombrearchivoNuevo);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


    } 
}
