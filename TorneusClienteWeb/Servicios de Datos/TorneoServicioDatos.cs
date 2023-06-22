using BDTorneus;
using Microsoft.AspNetCore.Components;
using Negocio.DTOs;
using Negocio.Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using TorneusClienteWeb.Servicios;

namespace TorneusClienteWeb.Servicios_de_Datos
{
    public class TorneoServicioDatos
    {

        private readonly HttpClient _httpClient;
        [Inject] private UsuarioServicio _usuarioServicio { get; set; }


        public TorneoServicioDatos(HttpClient httpClient, UsuarioServicio usuarioServicio)
        {
            _httpClient = httpClient;
            _usuarioServicio = usuarioServicio;
        }

        public async Task<List<TorneoDTO>> ObtenerTorneosOrganizador(int idUsuario)
        {
         
                try
                {
                    if (idUsuario < 1) throw new Exception("No usuario no existe");

                 string token = _usuarioServicio.ObtenerUsuarioLogueado().Token;
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.GetAsync($"api/Torneo/MisTorneos/{idUsuario}");

                    if (response.IsSuccessStatusCode)
                        {
                            var content = await response.Content.ReadAsStringAsync();
                            var torneos = JsonConvert.DeserializeObject<List<TorneoDTO>>(content);
                            return torneos;
                    }
                    else
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        throw new Exception(content);
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }

        }





    }
}
