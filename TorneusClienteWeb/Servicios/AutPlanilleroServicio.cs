using DTOs_Compartidos.DTOs;
using TorneusClienteWeb.Servicios_de_Datos;

namespace TorneusClienteWeb.Servicios
{
    public class AutPlanilleroServicio
    {
        List<AutorizacionPlanilleroDTO> AutorizacionPlanilleros = new();

        private readonly AutPlanilleroServicioDatos _autPlanilleroServicioDatos;
        public AutPlanilleroServicio(AutPlanilleroServicioDatos autPlanilleroServicioDatos)
        {
            _autPlanilleroServicioDatos = autPlanilleroServicioDatos;
        }



        public async Task<bool> RegistrarAutorizacion(AutorizacionPlanilleroDTO autorizacionDTO)
        {
            try
            {
                int autorizacionId = await _autPlanilleroServicioDatos.RegistrarAutorizacion(autorizacionDTO);
                if (autorizacionId > 0)
                {
                    autorizacionDTO.Id = autorizacionId;
                    // actualizar signalr
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<AutorizacionPlanilleroDTO>> ObtenerAutorizaciones()
        {
            return AutorizacionPlanilleros;
        }
        public async Task ObtenerAutorizacionesMarcacionesParaPlanilleros(int planilleroId)
        {
            try
            {
                var autorizaciones = await _autPlanilleroServicioDatos.ObtenerAutorizacionesMarcacionesParaPlanilleros(planilleroId);
                AutorizacionPlanilleros = autorizaciones;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task ObtenerAutorizacionesMarcacionesParaOrganizadores(int organizadorId)
        {
            try
            {
                var autorizaciones = await _autPlanilleroServicioDatos.ObtenerAutorizacionesMarcacionesParaOrganizadores(organizadorId);
                AutorizacionPlanilleros = autorizaciones;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public async Task<bool> EliminarAutorizacion(AutorizacionPlanilleroDTO autorizacionDTO)
        {
            try
            {
                var eliminado = await _autPlanilleroServicioDatos.EliminarAutorizacion(autorizacionDTO);
                return eliminado;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> EliminarAutorizacionesFinTorneo(int torneoId)
        {
            try
            {
                var eliminadosFinTorneo = await _autPlanilleroServicioDatos.EliminarAutorizacionesFinTorneo(torneoId);
                return eliminadosFinTorneo;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }






        }
}
