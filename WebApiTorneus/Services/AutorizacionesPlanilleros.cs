using DTOs_Compartidos.Models;

namespace WebApiTorneus.Services
{
    public class AutorizacionesPlanilleros
    {
        List<PlanilleroTorneo> PlanillerosAutorizados = new();

        public async Task<bool> AgregarPlanillleroAutorizado(PlanilleroTorneo planillero)
        {
            try
            {
                if (planillero == null) throw new Exception("No existe lista para autorizar");

                if (ExistenciaPlanilleroAutorizado(planillero)) throw new Exception("Hay usuarios repetidos en el listado actual");

                PlanillerosAutorizados.Add(planillero);

                return true;
            }
            catch (Exception ex)
            {
                 throw new Exception(ex.Message);
            }
        }

        public async Task<bool> QuitarAutorizacion(PlanilleroTorneo planillero)
        {
            if (planillero == null) return false;

            PlanillerosAutorizados.RemoveAll(rem => rem.UsuarioId == planillero.UsuarioId && rem.TorneoId == planillero.TorneoId);

            return true;
        }

        public async Task<bool> QuitarAutorizacionesFinTorneo(int torneoId)
        {
            if (torneoId < 0) return false;

            PlanillerosAutorizados.RemoveAll(rem => rem.TorneoId == torneoId);
            return true;
        }

        public async Task<bool> ExistenciaPlanilleroAutorizado(PlanilleroTorneo planillero)
        {
            bool existePlanillero = PlanillerosAutorizados.Any(busqueda => busqueda.UsuarioId == planillero.UsuarioId && busqueda.TorneoId == planillero.TorneoId);
            return existePlanillero;
        }

    }
}
