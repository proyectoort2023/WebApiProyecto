using DTOs_Compartidos.Models;

namespace WebApiTorneus.Services
{
    public class AutorizacionesPlanilleros
    {
        List<PlanilleroTorneo> PlanillerosAutorizados = new();


        public async Task<List<PlanilleroTorneo>> ObtenerPlanillerosSegunOrganizador(int usuarioOrganizadorId)
        {
            return PlanillerosAutorizados.Where(w => w.UsuarioIdOrganizador == usuarioOrganizadorId).ToList();
        }

        public async Task<bool> AgregarPlanillleroAutorizado(PlanilleroTorneo planillero)
        {
            try
            {
                if (planillero == null) throw new Exception("No existe lista para autorizar");

                if (await ValidarPlanillero(planillero)) throw new Exception("El usuario que quiere autorizar ya está autorizado en otro torneo el mismo dia");

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

            PlanillerosAutorizados.RemoveAll(rem => rem.UsuarioIdPlanillero == planillero.UsuarioIdPlanillero && rem.TorneoId == planillero.TorneoId);

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
            bool existePlanillero = PlanillerosAutorizados.Any(busqueda => busqueda.UsuarioIdPlanillero == planillero.UsuarioIdPlanillero 
                                                                        && busqueda.TorneoId == planillero.TorneoId);
            return existePlanillero;
        }

        public async Task<bool> ValidarPlanillero(PlanilleroTorneo planillero)
        {
            bool validarPlanillero = PlanillerosAutorizados.Any(busqueda => busqueda.UsuarioIdPlanillero == planillero.UsuarioIdPlanillero
                                                                        && busqueda.FechaTorneo == planillero.FechaTorneo);
            return validarPlanillero;
        }

    }
}
