using DTOs_Compartidos.Models;
using Microsoft.AspNetCore.Components;
using Negocio.DTOs;
using Newtonsoft.Json;
using System.Net.Http.Headers;
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


        public async Task<AccessTokenMercadoPago> ObtenerTokenMercadopago(string codigo)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.GetAsync($"api/MediosPagos/Mercadopago/AccessTokenVenderor/{codigo}");


                if (!response.IsSuccessStatusCode)
                {
                    var contentError = await response.Content.ReadAsStringAsync();
                    throw new Exception(contentError);
                }

                var content = await response.Content.ReadAsStringAsync();
                var resultado = JsonConvert.DeserializeObject<AccessTokenMercadoPago>(content);
                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
