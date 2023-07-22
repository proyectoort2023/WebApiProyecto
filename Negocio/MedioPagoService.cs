using Azure.Core;
using BDTorneus;
using DTOs_Compartidos.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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
        private readonly TorneoContext _db;
        private readonly IConfiguration _configuration;

        public MedioPagoService(TorneoContext db, IConfiguration configuration)
        {
            _db = db;
            _configuration = configuration;
        }

        public async Task<bool> ImplementarMercadoPagoVendedor(string codigo, int usuarioId)
        {
            try
            {
                AccessTokenMercadoPago accTokenMP = await ObtenerAccessTokenVendedor(codigo);

                if (accTokenMP == null) return false;

                bool resultado = await GuardarAccessTokenVendedor(accTokenMP, usuarioId);

                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }



        private async Task<AccessTokenMercadoPago> ObtenerAccessTokenVendedor(string codigo) {

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

        private async Task<bool> GuardarAccessTokenVendedor(AccessTokenMercadoPago accTokenMP, int usuarioId)
        {

            try
            {
                Usuario usuario = await _db.Usuarios.SingleOrDefaultAsync(us => us.Id == usuarioId);

                if (usuario == null) throw new Exception("No existe el usuario. W102");

                usuario.AccessTokenMercadopago = accTokenMP.access_token;
                usuario.AccessTokenRefreshMercadopago = accTokenMP.refresh_token;
                usuario.FechaCaducidadTokenMercadopago = ConvertirTimeSpanEnDatetime(accTokenMP.expires_in);

                int resultado = await _db.SaveChangesAsync();

                return resultado > 0;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private DateTime ConvertirTimeSpanEnDatetime(string timespanString)
        {
            long ticks = long.Parse(timespanString);

            TimeSpan timespan = TimeSpan.FromTicks(ticks);

            return DateTime.Today.Add(timespan);
        }


        public async Task<string> ObtenerAccesTokenVendedor(int usuarioId)
        {

            try
            {
                Usuario usuario = await _db.Usuarios.SingleOrDefaultAsync(us => us.Id == usuarioId);

                if (usuario == null) throw new Exception("No existe el usuario. W101");

                var token = usuario.AccessTokenMercadopago == null ? "" : usuario.AccessTokenMercadopago;
                return token;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }







    }
}
