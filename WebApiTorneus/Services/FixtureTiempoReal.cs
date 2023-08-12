using Negocio;
using Negocio.DTOs;

namespace WebApiTorneus.Services
{
    public class FixtureTiempoReal
    {
        List<PartidoDTO> fixturesTorneos = new List<PartidoDTO>();

        public async Task ActualizarPartidos(List<PartidoDTO> partidos)
        {
            foreach(var partido in partidos)
            {
                int indice = BuscarIndice(partido.Id);
                fixturesTorneos[indice] = partido;
            }
        }

        private int BuscarIndice(int partidoId)
        {
            return fixturesTorneos.FindIndex(f => f.Id == partidoId);
        }

        public List<PartidoDTO> ObtenerFixtureTorneo(int torneoId)
        {
            return fixturesTorneos.Where(w => w.Torneo.Id == torneoId).ToList();
        }

        public async Task CargarFixture(List<PartidoDTO> partidos)
        {
            fixturesTorneos.AddRange(partidos);
        }

        public async Task BorrarFixtureTiempoReak(int torneoId)
        {
            fixturesTorneos.RemoveAll(r => r.Torneo.Id == torneoId);
        }

    }
}
