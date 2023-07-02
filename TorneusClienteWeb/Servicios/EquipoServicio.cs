using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Negocio.DTOs;
using TorneusClienteWeb.Servicios_de_Datos;

namespace TorneusClienteWeb.Servicios
{
    public class EquipoServicio
    {
        private readonly TorneoServicioDatos _torneoServicioDatos;
        private readonly EquipoServicioDatos _equipoServicioDatos;
        private TorneoDTO TorneoSeleccionado = new();
        private List<TorneoDTO> Torneos = new();
        [Inject] private HubConnection _hubConnection { get; set; }

        public EquipoServicio(TorneoServicioDatos torneoServicioDatos, EquipoServicioDatos equipoServicioDatos, HubConnection hubConnection)
        {
            _torneoServicioDatos = torneoServicioDatos;
            _equipoServicioDatos = equipoServicioDatos;
            _hubConnection = hubConnection;
        }
    }
}
