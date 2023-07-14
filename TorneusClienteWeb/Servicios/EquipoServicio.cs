using DTOs_Compartidos.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Negocio.DTOs;
using System.Linq.Expressions;
using TorneusClienteWeb.Servicios_de_Datos;

namespace TorneusClienteWeb.Servicios
{
    public class EquipoServicio
    {
        private readonly EquipoServicioDatos _equipoServicioDatos;
        private List<EquipoDTO> Equipos = null;
        private EquipoDTO Equipo;

        [Inject] private HubConnection _hubConnection { get; set; }
        [Inject] private UsuarioServicio _usuarioServicio { get; set; }

        public EquipoServicio(EquipoServicioDatos equipoServicioDatos, HubConnection hubConnection, UsuarioServicio usuarioServicio)
        {
            _equipoServicioDatos = equipoServicioDatos;
            _hubConnection = hubConnection;
            _usuarioServicio = usuarioServicio;
        }


        public async Task<List<EquipoDTO>> ObtenerEquiposPorAdministrador()
        {
            try
            {
                int usuarioId = _usuarioServicio.ObtenerUsuarioLogueado().Id;

                if (Equipos == null)
                {
                    await CargarEquiposPorAdministrador(usuarioId);
                }
                return Equipos;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        private async Task CargarEquiposPorAdministrador(int usuarioId)
        {
            try
            {
                Equipos = await _equipoServicioDatos.ObtenerEquiposPorAdministrador(usuarioId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task RegistrarEquipo (EquipoDTO equipoDTO)
        {
            try
            {
                if (equipoDTO == null) throw new Exception("No existe el equipo");

                equipoDTO.Nombre = equipoDTO.Nombre.ToUpper().Trim();
                equipoDTO.Abreviatura = equipoDTO.Abreviatura.ToUpper().Trim();
                equipoDTO.UsuarioId = _usuarioServicio.ObtenerUsuarioLogueado().Id;

                int equipoId = await _equipoServicioDatos.CreaNuevoEquipo(equipoDTO);

                if (equipoId < 1) throw new Exception("No se puedo crear el equipo");

                await AgregarEquipoNuevoAListado(equipoDTO);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task AgregarEquipoNuevoAListado(EquipoDTO equipoDTO)
        {
            Equipos.Add(equipoDTO);
        }

        public int CantidadJugadoresEquipo(int equipoId)
        {
            int cantidad = 0;
            cantidad = Equipos.SingleOrDefault(w => w.Id == equipoId).Jugadores.Count();
            return cantidad;
        }


    }
}
