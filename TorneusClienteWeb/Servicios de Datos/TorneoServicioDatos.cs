using BDTorneus;
using DTOs_Compartidos.Models;
using Microsoft.AspNetCore.Components;
using Negocio.DTOs;
using Negocio.Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
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


        public async Task<List<TorneoDTO>> ObtenerTorneosVigentes()
        {

            try
            {
                string token = _usuarioServicio.ObtenerUsuarioLogueado().Token;
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.GetAsync("api/Torneo/TorneosVigentes");

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


        public async Task<bool> SuspenderTorneo(int torneoId)
        {
         
                try
                {
                    if (torneoId < 1) throw new Exception("No torneo no existe");

                 string token = _usuarioServicio.ObtenerUsuarioLogueado().Token;
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.GetAsync($"api/Torneo/Suspender/{torneoId}");

                    if (response.IsSuccessStatusCode)
                        {
                            var content = await response.Content.ReadAsStringAsync();
                            var resultado = JsonConvert.DeserializeObject<BoolModel>(content);
                            return resultado.Bandera;
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


        public async Task<TorneoDTO> CrearTorneoOrganizador(TorneoCreacionDTO torneoDTO)
        {

            try
            {
                if (torneoDTO == null) throw new Exception("No torneo no se ha podido crear");

                string token = _usuarioServicio.ObtenerUsuarioLogueado().Token;
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.PostAsJsonAsync($"api/Torneo/Crear", torneoDTO);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var torneo = JsonConvert.DeserializeObject<TorneoDTO>(content);
                    return torneo;
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



        public async Task<bool> EliminarTorneo(int torneoId)
        {

            try
            {
                if (torneoId < 1) throw new Exception("No torneo no existe");

                string token = _usuarioServicio.ObtenerUsuarioLogueado().Token;
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.DeleteAsync($"api/Torneo/Eliminar/{torneoId}");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var resultado = JsonConvert.DeserializeObject<BoolModel>(content);
                    return resultado.Bandera;
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


        public async Task<TorneoDTO> ModificarTorneoOrganizador(TorneoDTO torneoDTO)
        {

            try
            {
                if (torneoDTO == null) throw new Exception("No torneo no se ha podido crear");

                string token = _usuarioServicio.ObtenerUsuarioLogueado().Token;
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.PostAsJsonAsync($"api/Torneo/Modificar", torneoDTO);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var torneo = JsonConvert.DeserializeObject<TorneoDTO>(content);
                    return torneo;
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

        public async Task<bool> AbrirInscripcionesTorneoOrganizador(int torneoId)
        {

            try
            {
                if (torneoId < 1) throw new Exception("No existe el torneo");

                string token = _usuarioServicio.ObtenerUsuarioLogueado().Token;
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.GetAsync($"api/Torneo/HabilitarInscripcion/{torneoId}");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var torneo = JsonConvert.DeserializeObject<bool>(content);
                    return torneo;
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


        public async Task<bool> CerrarInscripcionesTorneoOrganizador(int torneoId)
        {

            try
            {
                if (torneoId < 1) throw new Exception("No existe el torneo");

                string token = _usuarioServicio.ObtenerUsuarioLogueado().Token;
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.GetAsync($"api/Torneo/CerrarInscripcion/{torneoId}");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var torneo = JsonConvert.DeserializeObject<bool>(content);
                    return torneo;
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

        public async Task<List<InscripcionDTO>> ObtenerInscripcionesTorneo(int torneoId)
        {
            try
            {
                string token = _usuarioServicio.ObtenerUsuarioLogueado().Token;
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.GetAsync($"api/Torneo/Inscripciones/{torneoId}");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var torneosInscripciones = JsonConvert.DeserializeObject<List<InscripcionDTO>>(content);
                    return torneosInscripciones;
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
