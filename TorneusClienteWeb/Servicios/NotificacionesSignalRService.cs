using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Negocio.DTOs;

namespace TorneusClienteWeb.Servicios
{
    public class NotificacionesSignalRService
    {
        [Inject] private TorneoServicio _torneoServicio { get; set; }
        [Inject] private FixtureServicio _fixtureServicio { get; set; }


        public NotificacionesSignalRService(TorneoServicio torneoServicio, FixtureServicio fixtureServicio)
        {
            _torneoServicio = torneoServicio;
            _fixtureServicio = fixtureServicio;
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
            RemoverItemTorneoLista(torneoId);
        }

        public async Task ModificarTorneoSignalR(TorneoDTO torneoDTO)
        {
            int idTorneo = torneoDTO.Id;
            RemoverItemTorneoLista(idTorneo);
            AgregarTorneoALista(torneoDTO);
           await _torneoServicio.SeleccionTorneo(idTorneo);
           await _torneoServicio.OrdenarTorneoPorFecha();
        }

        private void RemoverItemTorneoLista(int torneoId)
        {
            _torneoServicio.ObtenerTorneos().RemoveAll(r => r.Id == torneoId);
        }

        private void AgregarTorneoALista(TorneoDTO torneoDTO)
        {
            _torneoServicio.ObtenerTorneos().Add(torneoDTO);
        }

        public void AbrirCerrarInscripciones(int torneoId, bool habilitacionTorneo)
        {
            int posicion = _torneoServicio.BuscarIndiceTorneo(torneoId);
            _torneoServicio.ObtenerTorneos()[posicion].HabilitacionInscripcion = habilitacionTorneo;
        }

        #region Fixture
        public async Task ActualizarPartidosSignalR(List<PartidoDTO> partidos)
        {
            if (_torneoServicio.ObtenerTorneoActual().Id == partidos[0].Torneo.Id)
            {
                foreach (var partido in partidos)
                {
                    await _fixtureServicio.SetPartidos(partido);
                }
                await _fixtureServicio.ActualizarTiempoPromedioPartidos();
                await _fixtureServicio.ActualizarListadoPartidosFront();
            }
          
        }
     
        #endregion




    }
}
