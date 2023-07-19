using Azure.Core;
using DTOs_Compartidos.Models;
using Negocio.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio
{
    public class MedioPagoService
    {
       

        public async Task<AccessTokenMercadoPago> ObtenerAccessTokenVendedor(string codigo) {

            try
            {
                HttpClient clienteHttp = new HttpClient();

                var formContent = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    {"client_id", "3122573609429367"},
                    {"client_secret", "6lh07YifyhOdIKsxhxHssV0m4GtFnhqk"},
                    {"grant_type","authorization_code"},
                    {"code", codigo},
                    {"redirect_uri","https://torneus.azurewebsites.net/ORGANIZADOR/Mercadopago"}
                });


                HttpResponseMessage response = await clienteHttp.PostAsync("https://api.mercadopago.com/oauth/token", formContent);

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    var resultado = JsonConvert.DeserializeObject<AccessTokenMercadoPago>(content);
                    return resultado;
                }
                else
                {
                    throw new Exception($"No se ha podido interconectar a su marketplace de vendedor. {response.StatusCode} ");
                }


            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
