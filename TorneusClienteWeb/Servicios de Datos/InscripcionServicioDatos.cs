using Microsoft.AspNetCore.Components;
using Negocio.DTOs;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using TorneusClienteWeb.Servicios;

namespace TorneusClienteWeb.Servicios_de_Datos
{
    public class InscripcionServicioDatos
    {
        private readonly HttpClient _httpClient;
        [Inject] private UsuarioServicio _usuarioServicio { get; set; }


        public InscripcionServicioDatos(HttpClient httpClient, UsuarioServicio usuarioServicio)
        {
            _httpClient = httpClient;
            _usuarioServicio = usuarioServicio;
        }


        public async Task<List<InscripcionDTO>> ObtenerInscripcionesDeUsuario(int idUsuario)
        {

            try
            {
                if (idUsuario < 1) throw new Exception("No usuario no existe");

                string token = _usuarioServicio.ObtenerUsuarioLogueado().Token;
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.GetAsync($"api/Inscripcion/Listado/{idUsuario}");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var inscripciones = JsonConvert.DeserializeObject<List<InscripcionDTO>>(content);
                    return inscripciones;
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


        public async Task<InscripcionDTO> RegistrarNuevaInscripcion(InscripcionDTO inscripcionDTO)
        {

            try
            {
                string token = _usuarioServicio.ObtenerUsuarioLogueado().Token;
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.PostAsJsonAsync("api/Inscripcion/Agregar", inscripcionDTO);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var inscripcion = JsonConvert.DeserializeObject<InscripcionDTO>(content);
                    return inscripcion;
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
