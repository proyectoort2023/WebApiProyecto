using Negocio.DTOs;
using TorneusClienteWeb.Servicios_de_Datos;

namespace TorneusClienteWeb.Servicios
{
    public class TorneoServicio
    {
        private readonly TorneoServicioDatos _torneoServicioDatos;
        private TorneoDTO TorneoSeleccionado = new();
        private List<TorneoDTO> Torneos = new();

        public TorneoServicio(TorneoServicioDatos torneoServicioDatos)
        {
            _torneoServicioDatos = torneoServicioDatos;
        }

        public async Task ListadoTorneosOrganizadorData(int idUsuario)
        {
            try
            {
                var torneosOrganizador = await _torneoServicioDatos.ObtenerTorneosOrganizador(idUsuario);
                Torneos = torneosOrganizador;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public TorneoDTO ObtenerTorneoActual()
        {
            return TorneoSeleccionado;
        }

        public async Task<List<TorneoDTO>> ObtenerTorneosOrganizador(int idUsuario)
        {
            try
            {
                if (Torneos.Count < 1)
                {
                    await ListadoTorneosOrganizadorData(idUsuario);
                }
                return Torneos;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<TorneoDTO> CrearTorneoOrganizador(TorneoCreacionDTO torneoDTO)
        {
            try
            {
                return await _torneoServicioDatos.CrearTorneoOrganizador(torneoDTO);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task SeleccionTorneo(int torneoId)
        {
            TorneoSeleccionado = Torneos.SingleOrDefault(to => to.Id == torneoId);
        }

        public async Task SuspenderTorneoSignalR(int torneoId)
        {
            var cambioSuspendido = Torneos.SingleOrDefault(w => w.Id == torneoId);
            if (cambioSuspendido != null)
            {
                cambioSuspendido.Suspendido = true;
            }
        }

        public async Task<bool> SuspenderTorneo(int torneoId)
        {
            try
            {
               bool suspendido = await _torneoServicioDatos.SuspenderTorneo(torneoId);
                return suspendido;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
           
        }

        public async Task<bool> EliminarTorneo(int torneoId)
        {
            try
            {
                bool eliminado = await _torneoServicioDatos.EliminarTorneo(torneoId);
                return eliminado;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public async Task EliminarTorneoSignalR(int torneoId)
        {
            Torneos.RemoveAll(r => r.Id == torneoId);
        }









    }
}
