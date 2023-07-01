﻿using BDTorneus;
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

        public TorneoDTO ObtenerTorneoActual()
        {
            return TorneoSeleccionado;
        }
        public List<TorneoDTO> ObtenerTorneos()
        {
            return Torneos;
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


        public async Task<TorneoDTO> ModificarTorneoOrganizador(TorneoDTO torneoDTO)
        {
            try
            {
                var torneoModificado = await _torneoServicioDatos.ModificarTorneoOrganizador(torneoDTO);
                await _hubConnection.SendAsync("EnviarNotificacionModificacionTorneo", torneoDTO);
                return torneoModificado;
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


    }
}
