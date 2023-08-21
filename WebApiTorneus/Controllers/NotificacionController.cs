using AutoMapper;
using BDTorneus;
using DTOs_Compartidos.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Negocio;
using Negocio.DTOs;
using WebApiTorneus.HubSignalR;
using WebApiTorneus.Services;
using static Utilidades.Util;

namespace WebApiTorneus.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificacionController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly NotificacionService _notificacionService;

        private readonly IHubContext<TorneusHub> _torneoHub;

        public NotificacionController(IMapper mapper, NotificacionService notificacionService, IHubContext<TorneusHub> torneoHub)
        {
            _mapper = mapper;
            _notificacionService = notificacionService;
             _torneoHub = torneoHub;
        }


        /// <summary>
        /// Permite el registro de una nueva notificacion a un usuario
        /// </summary>
        /// <remarks>
        /// Este endpoint permite el registro de una nueva notificacion a un usuario
        /// </remarks>
        /// <response code="200">OK. Registrado </response>
        /// <response code="400">Validaciones varias no conformadas</response>
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "ORGANIZADOR,PLANILLERO")]
        [HttpPost("Registrar")]
        public async Task<IActionResult> RegistrarNotificacion([FromBody] NotificacionDTO notificacionDTO)
        {
            try
            {
                var notificacion = _mapper.Map<NotificacionDTO, Notificacion>(notificacionDTO);

                NotificacionDTO registrada = _mapper.Map<Notificacion, NotificacionDTO>(await _notificacionService.Registrar(notificacion));

                if (registrada != null)
                {
                    await _torneoHub.Clients.All.SendAsync("RecibirNuevaNotificacion", registrada);
                    return Ok(registrada.Id > 0);
                }
                else
                {
                    return BadRequest("No se puedo registrar");
                }
                
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        /// <summary>
        /// Permite obtener las notificaciones segun el rol del usuario
        /// <remarks>
        /// Este endpoint permite obtener las notificaciones segun el rol del usuario
        /// </remarks>
        /// <response code="200">OK </response>
        /// <response code="400">Validaciones varias no conformadas</response>
        [ProducesResponseType(typeof(List<NotificacionDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize]
        [HttpPost("ObtenerNotificaciones")]
        public async Task<IActionResult> ObtenerNotificaciones(UsuarioLogueado usuario)
        {
            try
            {
                if (usuario == null) throw new Exception("No se ha recibido ningun usuario");

                var listaNotificaciones = _mapper.Map<List<Notificacion>, List<NotificacionDTO>>(await _notificacionService.ObtenerSegunUsuario(usuario));

                return Ok(listaNotificaciones);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        /// <summary>
        /// Permite eliminar todas las notificaciones luego del termino de un torneo
        /// <remarks>
        /// Este endpoint permite eliminar todas las notificaciones luego del termino de un torneo
        /// </remarks>
        /// <response code="200">OK </response>
        /// <response code="400">Validaciones varias no conformadas</response>
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "ORGANIZADOR,PLANILLERO")]
        [HttpDelete("BorrarNotificacionesFinTorneo/{torneoId}")]
        public async Task<IActionResult> BorrarNotificacionesFinTorneo(int torneoId)
        {
            try
            {
                if (torneoId < 0 ) throw new Exception("No se ha recibido ningun torneo");

                bool borrados = await _notificacionService.BorrarNotificacionesTerminoPartido(torneoId);

                return Ok(borrados);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }







    }
}
