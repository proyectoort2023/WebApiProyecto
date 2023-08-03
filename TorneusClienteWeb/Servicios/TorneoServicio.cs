using BDTorneus;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Negocio.DTOs;
using TorneusClienteWeb.Servicios_de_Datos;

namespace TorneusClienteWeb.Servicios
{
    public class TorneoServicio
    {
        private readonly TorneoServicioDatos _torneoServicioDatos;
        private TorneoDTO TorneoSeleccionado = new();
        private List<TorneoDTO> Torneos = new();
        [Inject] private HubConnection _hubConnection { get;set; }

        public TorneoServicio(TorneoServicioDatos torneoServicioDatos, HubConnection hubConnection)
        {
            _torneoServicioDatos = torneoServicioDatos;
            _hubConnection = hubConnection;
        }

        #region Métodos para organizadores
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

        public List<TorneoDTO> ObtenerTorneos()
        {
            return Torneos;
        }

        public TorneoDTO ObtenerTorneoActual()
        {
            return TorneoSeleccionado;
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

       

        public async Task<bool> SuspenderTorneo(int torneoId)
        {
            try
            {
                bool suspendido = await _torneoServicioDatos.SuspenderTorneo(torneoId);
                await _hubConnection.SendAsync("EnviarNotificacionSuspensionTorneo", suspendido, torneoId);
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


        public async Task<bool> ModificarTorneoOrganizador(TorneoDTO torneoDTO)
        {
            try
            {
                var torneoModificado = await _torneoServicioDatos.ModificarTorneoOrganizador(torneoDTO);
                await _hubConnection.SendAsync("EnviarNotificacionModificacionTorneo", torneoModificado);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task OrdenarTorneoPorFecha()
        {
          if (Torneos.Count > 0)
            {
                Torneos = Torneos.OrderByDescending(o => o.Fecha).ToList();
            }
        }

        public async Task<bool> AbrirInscripcionesTorneoOrganizador(TorneoDTO torneoDTO)
        {
            try
            {
                bool resultado = await _torneoServicioDatos.AbrirInscripcionesTorneoOrganizador(torneoDTO.Id);
                if (!resultado) throw new Exception("No se ha podiddo abrir la inscripcion del torneo seleccionado");

                TorneoSeleccionado.HabilitacionInscripcion = true;
                await _hubConnection.SendAsync("EnviarAperturaCierreTorneo", torneoDTO.Id, TorneoSeleccionado.HabilitacionInscripcion); 
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> CerrarInscripcionesTorneoOrganizador(TorneoDTO torneoDTO)
        {
            try
            {
                bool resultado = await _torneoServicioDatos.CerrarInscripcionesTorneoOrganizador(torneoDTO.Id);
                if (!resultado) throw new Exception("No se ha podiddo cerrar la inscripcion del torneo seleccionado");

                TorneoSeleccionado.HabilitacionInscripcion = false;
                await _hubConnection.SendAsync("EnviarAperturaCierreTorneo", torneoDTO.Id, TorneoSeleccionado.HabilitacionInscripcion);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public int BuscarIndiceTorneo(int torneoId)
        {
            return Torneos.FindIndex(f => f.Id == torneoId);
        }

        #endregion


        #region Métodos para admin de equipos

        private async Task ListadoTorneosVigentesData()
        {
            try
            {
                var torneosVigentes = await _torneoServicioDatos.ObtenerTorneosVigentes();
                Torneos = torneosVigentes;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        public async Task<List<TorneoDTO>> ObtenerTorneosVigentes()
        {
            try
            {
                if (Torneos.Count < 1)
                {
                    await ListadoTorneosVigentesData();
                }
                return Torneos;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion


        public async Task<List<EquipoDTO>> ObtenerEquiposInscripciones()
        {
            try
            {
                int torneoId = TorneoSeleccionado.Id;

                List<InscripcionDTO> inscripciones = await _torneoServicioDatos.ObtenerInscripcionesTorneo(torneoId);

                List<EquipoDTO> equipos = inscripciones.Select(ins => ins.Equipo).ToList();
                return equipos;
            }
            catch (Exception)
            {

                throw;
            }
           
        }

    }
}
