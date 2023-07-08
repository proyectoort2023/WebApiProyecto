using DTOs_Compartidos.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Negocio.DTOs;
using TorneusClienteWeb.Servicios_de_Datos;

namespace TorneusClienteWeb.Servicios
{
    public class JugadorServicio
    {
        private readonly JugadorServicioDatos _jugadorServicioDatos;

        [Inject] private HubConnection _hubConnection { get; set; }
        [Inject] private UsuarioServicio _usuarioServicio { get; set; }

        public JugadorServicio(HubConnection hubConnection, UsuarioServicio usuarioServicio, JugadorServicioDatos jugadorServicioDatos)
        {
            _hubConnection = hubConnection;
            _usuarioServicio = usuarioServicio;
            _jugadorServicioDatos = jugadorServicioDatos;
        }

        public async Task<List<JugadorDTO>> ObtenerJugadoresTodos()
        {
            try
            {
                var jugadores = await _jugadorServicioDatos.ObtenerJugadoresTodos();
                return jugadores;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<int> RegistrarJugador(JugadorDTO jugadorDTO)
        {
            try
            {
                if (jugadorDTO == null) throw new Exception("Hay datos varios del jugador");

                int idJugador = await _jugadorServicioDatos.RegistrarJugador(jugadorDTO);
                return idJugador;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }





    }
}
