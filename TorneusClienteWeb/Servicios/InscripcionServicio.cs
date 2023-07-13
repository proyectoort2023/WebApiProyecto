using Microsoft.AspNetCore.Components;
using Negocio.DTOs;

namespace TorneusClienteWeb.Servicios
{
    public class InscripcionServicio
    {
        [Inject] private TorneoServicio _torneoServicio { get; set; }
        [Inject] private UsuarioServicio _usuarioServicio { get; set; }

        private List<InscripcionDTO> _inscripciones { get; set; } = new List<InscripcionDTO>();
        private InscripcionDTO _inscripcionSeleccionado;

        public InscripcionServicio(TorneoServicio torneoServicio)
        {
            _torneoServicio = torneoServicio;
        }

        private async Task CargaInscripciones()
        {
            //por web api inicial
        }
        public async Task<List<InscripcionDTO>> ObtenerInscripciones()
        {
          if (_inscripciones.Count == 0)
            {
                await CargaInscripciones();
            }
          return _inscripciones;
        }

        public async Task AgregarInscripcionNueva(int equipoId, double monto, string modalidad)
        {
            //por web api inicial
        }

        public bool ExisteInscripcion(int torneoId) => _inscripciones.Any(a => a.Torneo.Id == torneoId);







    }
}
