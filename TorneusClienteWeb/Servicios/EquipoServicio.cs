using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Negocio.DTOs;
using TorneusClienteWeb.Servicios_de_Datos;

namespace TorneusClienteWeb.Servicios
{
    public class EquipoServicio
    {
        private readonly EquipoServicioDatos _equipoServicioDatos;
        private List<EquipoDTO> Equipos = null;
        private EquipoDTO Equipo;

        [Inject] private HubConnection _hubConnection { get; set; }
        [Inject] private UsuarioServicio _usuarioServicio { get; set; }

        public EquipoServicio(EquipoServicioDatos equipoServicioDatos, HubConnection hubConnection, UsuarioServicio usuarioServicio)
        {
            _equipoServicioDatos = equipoServicioDatos;
            _hubConnection = hubConnection;
            _usuarioServicio = usuarioServicio;
        }


        public async Task<List<EquipoDTO>> ObtenerEquiposPorAdministrador()
        {
            try
            {
                int usuarioId = _usuarioServicio.ObtenerUsuarioLogueado().Id;

                if (Equipos == null)
                {
                    await CargarEquiposPorAdministrador(usuarioId);
                }
                return Equipos;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        private async Task CargarEquiposPorAdministrador(int usuarioId)
        {
            try
            {
                Equipos = await _equipoServicioDatos.ObtenerEquiposPorAdministrador(usuarioId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<JugadorDTO>> ObtenerJugadoresTodos()
        {
            try
            {
                var jugadores =  await _equipoServicioDatos.ObtenerJugadoresTodos();
                return jugadores;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }




    }
}
