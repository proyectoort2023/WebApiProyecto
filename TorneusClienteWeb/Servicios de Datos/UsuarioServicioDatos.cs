using Negocio.DTOs;
using Negocio.Models;
using Newtonsoft.Json;
using System.Net.Http.Json;

namespace TorneusClienteWeb.Servicios_de_Datos
{
    public class UsuarioServicioDatos
    {
        private readonly HttpClient _httpClient;
        public UsuarioServicioDatos(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> LoginUsuario(LoginDTO login)
        {
            try
            {
                if (login == null) throw new Exception("No existe el modelo");

                var response = await _httpClient.PostAsJsonAsync("api/Usuario/Login", login);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var token = JsonConvert.DeserializeObject<TokenModel>(content);
                    return token.Token;
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


        public async Task<string> RegistroUsuario(RegistroDTO registroDTO)
        {
            try
            {
                if (registroDTO == null) throw new Exception("No existe el modelo");

                var response = await _httpClient.PostAsJsonAsync("api/Usuario/Registro", registroDTO);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var token = JsonConvert.DeserializeObject<TokenModel>(content);
                    return token.Token;
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



        public async Task<bool> ObtenerHabilitacionVendedorMarketplace(int usuarioId)
        {
            try
            {
                if (usuarioId < 0) throw new Exception("No existe el modelo");

                var response = await _httpClient.GetAsync($"api/Usuario/HabilitadoVendedorMarketPlace/{usuarioId}");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var resultado = JsonConvert.DeserializeObject<bool>(content);
                    return resultado;
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

        //public async Task<List<UsuarioDTO>> ObtenerTodos(bool filtro)
        //{
        //    var response = await _httpClient.GetAsync($"api/Usuario/{filtro}");

        //    if (response.IsSuccessStatusCode)
        //    {
        //        var content = await response.Content.ReadAsStringAsync();
        //        var entidad = JsonConvert.DeserializeObject<List<UsuarioDTO>>(content);
        //        return entidad;
        //    }
        //    else
        //    {
        //        var content = await response.Content.ReadAsStringAsync();
        //        throw new Exception(content);
        //    }
        //}

        //public async Task<int> Nuevo(UsuarioDTO usuarioDTO)
        //{
        //    var response = await _httpClient.PostAsJsonAsync("api/Usuario/nuevo", usuarioDTO);

        //    if (response.IsSuccessStatusCode)
        //    {
        //        var content = await response.Content.ReadAsStringAsync();
        //        var entidad = JsonConvert.DeserializeObject<int>(content);
        //        return entidad;
        //    }
        //    else
        //    {
        //        var content = await response.Content.ReadAsStringAsync();
        //        throw new Exception(content);
        //    }
        //}

        //public async Task<bool> Editar(UsuarioDTO usuarioDTO)
        //{
        //    var response = await _httpClient.PostAsJsonAsync("api/Usuario/editar", usuarioDTO);

        //    if (response.IsSuccessStatusCode)
        //    {
        //        var content = await response.Content.ReadAsStringAsync();
        //        var entidad = JsonConvert.DeserializeObject<bool>(content);
        //        return entidad;
        //    }
        //    else
        //    {
        //        var content = await response.Content.ReadAsStringAsync();
        //        throw new Exception(content);
        //    }
        //}





    }
}
