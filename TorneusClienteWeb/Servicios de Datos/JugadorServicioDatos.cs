using DTOs_Compartidos.Models;
using Microsoft.AspNetCore.Components;
using Negocio.DTOs;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using TorneusClienteWeb.Servicios;

namespace TorneusClienteWeb.Servicios_de_Datos
{
    public class JugadorServicioDatos
    {
        private readonly HttpClient _httpClient;
        [Inject] private UsuarioServicio _usuarioServicio { get; set; }

        private string token;

        public JugadorServicioDatos(HttpClient httpClient, UsuarioServicio usuarioServicio)
        {
            _httpClient = httpClient;
            _usuarioServicio = usuarioServicio;
            token = _usuarioServicio.ObtenerUsuarioLogueado().Token;
        }


        public async Task<List<JugadorDTO>> ObtenerJugadoresTodos()
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.GetAsync($"api/Jugador/ObtenerJugadores");


                if (!response.IsSuccessStatusCode)
                {
                    var contentError = await response.Content.ReadAsStringAsync();
                    throw new Exception(contentError);
                }

                var content = await response.Content.ReadAsStringAsync();
                var resultado = JsonConvert.DeserializeObject<List<JugadorDTO>>(content);
                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }



        public async Task<int> RegistrarJugador(JugadorDTO jugadorDTO)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.PostAsJsonAsync($"api/Jugador/Crear", jugadorDTO);


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


       


    }
}
