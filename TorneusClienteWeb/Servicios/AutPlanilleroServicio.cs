using DTOs_Compartidos.DTOs;
using Negocio.DTOs;
using TorneusClienteWeb.Servicios_de_Datos;
using Utilidades;

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
                    AutorizacionPlanilleros.Add(autorizacionDTO);
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

        public async Task<List<AutorizacionPlanilleroDTO>> ObtenerAutorizaciones(UsuarioLogueado usuarioLogueado)
        {
            if (AutorizacionPlanilleros.Count > 0)
            {
                return AutorizacionPlanilleros;
            }
            else
            {
                if (usuarioLogueado != null)
                {
                    if (usuarioLogueado.Rol == Util.Roles.PLANILLERO.ToString())
                    {
                        await ObtenerAutorizacionesMarcacionesParaPlanilleros(usuarioLogueado.Id);
                    }
                    if (usuarioLogueado.Rol == Util.Roles.ORGANIZADOR.ToString())
                    {
                        await ObtenerAutorizacionesMarcacionesParaOrganizadores(usuarioLogueado.Id);
                    }
                }
                return AutorizacionPlanilleros;
            }
        }


        private async Task ObtenerAutorizacionesMarcacionesParaPlanilleros(int planilleroId)
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

        private async Task ObtenerAutorizacionesMarcacionesParaOrganizadores(int organizadorId)
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
                if (eliminado) AutorizacionPlanilleros.RemoveAll(r => r.Id == autorizacionDTO.Id);

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

        public async Task<AutorizacionPlanilleroDTO> ObtenerUsuarioAAutorizar(string mail)
        {
            try
            {
                AutorizacionPlanilleroDTO usuarioEncontrado = await _autPlanilleroServicioDatos.ObtenerUsuarioAAutorizar(mail);
                return usuarioEncontrado;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> ExistenciaAutorizacionPlanillero(int torneoId)
        {
            return AutorizacionPlanilleros.Any(a => a.TorneoId == torneoId);
        }


    }
}
