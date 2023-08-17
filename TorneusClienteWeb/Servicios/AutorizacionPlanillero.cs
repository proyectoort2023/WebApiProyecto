using DTOs_Compartidos.Models;
using TorneusClienteWeb.Servicios_de_Datos;

namespace TorneusClienteWeb.Servicios
{
    public class AutorizacionPlanillero
    {

        private readonly AutorizacionPlanilleroDatos _autorizacionPlanilleroDatos;

        public AutorizacionPlanillero(AutorizacionPlanilleroDatos autorizacionPlanilleroDatos)
        {
            _autorizacionPlanilleroDatos = autorizacionPlanilleroDatos;
        }


        public async Task<bool> AgregarPlanilleroAutorizado(PlanilleroTorneo planilleroTorneo)
        {
        }

        public async Task<bool> QuitarAutorizacionPlanillero(PlanilleroTorneo planilleroTorneo)
        {
        }

        public async Task<bool> QuitarAutorizacionesPorCierreTorneo(int torneoId)
        {
        }

        public async Task<bool> ExistenciaAutorizacionPlanillero(PlanilleroTorneo planilleroToreno)
        {
        }

        public async Task<List<PlanilleroTorneo>> ListadoPlanillerosAutorizadosOrganizador(int usuarioOrganizadorId)
        {

        }













        }
    }
