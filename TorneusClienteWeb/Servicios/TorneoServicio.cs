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

        private async Task ObtenerTorneosOrganizadorData(int idUsuario)
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
                    await ObtenerTorneosOrganizadorData(idUsuario);
                }
                return Torneos;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
