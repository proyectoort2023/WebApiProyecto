using DTOs_Compartidos.Models;
using Microsoft.AspNetCore.Components;
using Negocio.DTOs;
using Negocio.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using TorneusClienteWeb.Servicios;

namespace TorneusClienteWeb.Servicios_de_Datos
{
    public class EquipoServicioDatos
    {
        private readonly HttpClient _httpClient;
        [Inject] private UsuarioServicio _usuarioServicio { get; set; }

        private string token;

        public EquipoServicioDatos(HttpClient httpClient, UsuarioServicio usuarioServicio)
        {
            _httpClient = httpClient;
            _usuarioServicio = usuarioServicio;
            token = _usuarioServicio.ObtenerUsuarioLogueado().Token;
        }

        public async Task<List<EquipoDTO>> ObtenerEquiposPorAdministrador(int usuarioId)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.GetAsync($"api/Equipo/ObtenerEquipos/{usuarioId}");


                if (!response.IsSuccessStatusCode)
                {
                    var contentError = await response.Content.ReadAsStringAsync();
                    throw new Exception(contentError);
                }

                var content = await response.Content.ReadAsStringAsync();
                var resultado = JsonConvert.DeserializeObject<List<EquipoDTO>>(content);
                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task<int> CreaNuevoEquipo(EquipoDTO equipoDTO)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.PostAsJsonAsync("api/Equipo/Crear", equipoDTO);


                if (!response.IsSuccessStatusCode)
                {
                    var contentError = await response.Content.ReadAsStringAsync();
                    throw new Exception(contentError);
                }

                var content = await response.Content.ReadAsStringAsync();
                var resultado = JsonConvert.DeserializeObject<int>(content);
                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task<EquipoDTO> ModificarEquipo(EquipoDTO equipoDTO)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.PostAsJsonAsync("api/Equipo/Modificar", equipoDTO);


                if (!response.IsSuccessStatusCode)
                {
                    var contentError = await response.Content.ReadAsStringAsync();
                    throw new Exception(contentError);
                }

                var content = await response.Content.ReadAsStringAsync();
                var resultado = JsonConvert.DeserializeObject<EquipoDTO>(content);
                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }





    }
}
