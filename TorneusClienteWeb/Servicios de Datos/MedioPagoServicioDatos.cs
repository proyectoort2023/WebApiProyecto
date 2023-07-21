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
    public class MedioPagoServicioDatos
    {

        private readonly HttpClient _httpClient;
        [Inject] private UsuarioServicio _usuarioServicio { get; set; }

        private string token;
        public MedioPagoServicioDatos(HttpClient httpClient, UsuarioServicio usuarioServicio)
        {
            _httpClient = httpClient;
            _usuarioServicio = usuarioServicio;
            token = _usuarioServicio.ObtenerUsuarioLogueado().Token;
        }


        public async Task<bool> ImplementarMercadoPagoVendedorData(MpAuthVendedor mp, string token)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.PostAsJsonAsync("api/MediosPagos/Mercadopago/AccessTokenVendedor/", mp);


                if (!response.IsSuccessStatusCode)
                {
                    var contentError = await response.Content.ReadAsStringAsync();
                    throw new Exception(contentError);
                }

                var content = await response.Content.ReadAsStringAsync();
                var resultado = JsonConvert.DeserializeObject<bool>(content);
                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task<string> ObtenerAccessTokenVendedor(int usuarioVendedorId)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.GetAsync($"api/MediosPagos/Mercadopago/GetTokenVendedor/{usuarioVendedorId}");


                if (!response.IsSuccessStatusCode)
                {
                    var contentError = await response.Content.ReadAsStringAsync();
                    throw new Exception(contentError);
                }

                var content = await response.Content.ReadAsStringAsync();
                var resultado = JsonConvert.DeserializeObject<StringModel>(content);
                return resultado.Data;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
