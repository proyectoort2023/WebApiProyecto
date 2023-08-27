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
            try
            {
              return await _autorizacionPlanilleroDatos.AgregarPlanilleroAutorizado(planilleroTorneo);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> QuitarAutorizacionPlanillero(PlanilleroTorneo planilleroTorneo)
        {
            try
            {
                return await _autorizacionPlanilleroDatos.QuitarAutorizacionPlanillero(planilleroTorneo);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> QuitarAutorizacionesPorCierreTorneo(int torneoId)
        {
            try
            {
                return await _autorizacionPlanilleroDatos.QuitarAutorizacionesPorCierreTorneo(torneoId);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> ExistenciaAutorizacionPlanillero(int torneoId, int planilladorId)
        {
            try
            {
                PlanilleroTorneo planilleroToreno = new()
                {
                      TorneoId = torneoId,
                      UsuarioIdPlanillero = planilladorId
                };
                return await _autorizacionPlanilleroDatos.ExistenciaAutorizacionPlanillero(planilleroToreno);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<List<PlanilleroTorneo>> ListadoPlanillerosAutorizadosOrganizador(int usuarioOrganizadorId)
        {
            try
            {
                return await _autorizacionPlanilleroDatos.ListadoPlanillerosAutorizadosOrganizador(usuarioOrganizadorId);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }



        public async Task<DatosPlanillero> ObtenerIdPlanillero(string mail)
        {
            try
            {
                return await _autorizacionPlanilleroDatos.ObtenerUsuarioIdPlanillero(mail);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }









    }
    }
