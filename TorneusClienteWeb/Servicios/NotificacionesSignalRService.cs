using BDTorneus;
using Microsoft.AspNetCore.Components;
using Negocio.DTOs;
using TorneusClienteWeb.Servicios_de_Datos;

namespace TorneusClienteWeb.Servicios
{
    public class NotificacionesSignalRService
    {
        [Inject] private TorneoServicio _torneoServicio { get; set; }

        public NotificacionesSignalRService(TorneoServicio torneoServicio)
        {
            _torneoServicio = torneoServicio;
        }

        public async Task SuspenderTorneoSignalR(int torneoId)
        {
            var cambioSuspendido = _torneoServicio.ObtenerTorneos().SingleOrDefault(w => w.Id == torneoId);
            if (cambioSuspendido != null)
            {
                cambioSuspendido.Suspendido = true;
            }
        }

        public async Task EliminarTorneoSignalR(int torneoId)
        {
            RemoverItmeTorneoLista(torneoId);
        }

        public async Task ModificarTorneoSignalR(TorneoDTO torneoDTO)
        {
            RemoverItmeTorneoLista(torneoDTO.Id);
            AgregarItemTorneoLista(torneoDTO);
           await _torneoServicio.OrdenarTorneoPorFecha();
        }

        private void RemoverItmeTorneoLista(int torneoId)
        {
            _torneoServicio.ObtenerTorneos().RemoveAll(r => r.Id == torneoId);
        }

        private void AgregarItemTorneoLista(TorneoDTO torneoDTO)
        {
            _torneoServicio.ObtenerTorneos().Add(torneoDTO);
        }









    }
}
