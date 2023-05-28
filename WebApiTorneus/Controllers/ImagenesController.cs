using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Negocio;
using Negocio.Models;
using static Utilidades.Util;

namespace WebApiTorneus.Controllers
{
    [Authorize(Roles = "ORGANIZADOR,EQUIPO")]
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

        [HttpPost("SubirImagen")]
        public async Task<IActionResult> SubirImagen([FromForm] ImagenModel imagenModel)
        {
            if (imagenModel.Archivo == null) return BadRequest("No ha seleccionado una imagen");


            string nombreArchivo = await _imagenService.SubirImagen(_conexionStorage, _storageNameAzure, imagenModel.Archivo);
            if (nombreArchivo == null) return NoContent();

            return Ok(nombreArchivo); 
        }

        [HttpPost("ActualizarImagen")]
        public async Task<IActionResult> ActualizarImagen([FromForm] ImagenModel imagenModel)
        {
            if (imagenModel.Archivo == null) return BadRequest("No ha seleccionado una imagen");

            string nombreArchivo = await _imagenService.ActualizarImagen(_conexionStorage, _storageNameAzure, imagenModel.Archivo, imagenModel.NombreArchivoAnterior);
            if (nombreArchivo == null) return NoContent();

            return Ok(nombreArchivo);
        }


    } 
}
