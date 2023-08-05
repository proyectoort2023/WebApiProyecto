﻿using DTOs_Compartidos.Models;
using Microsoft.AspNetCore.Components;
using Negocio.DTOs;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using TorneusClienteWeb.Servicios;

namespace TorneusClienteWeb.Servicios_de_Datos
{
    public class FixtureServicioDatos
    {
        private readonly HttpClient _httpClient;
        [Inject] private UsuarioServicio _usuarioServicio { get; set; }


        public FixtureServicioDatos(HttpClient httpClient, UsuarioServicio usuarioServicio)
        {
            _httpClient = httpClient;
            _usuarioServicio = usuarioServicio;
        }


        public async Task<List<PartidoDTO>> CrearFixtureTorneo(PartidosTorneo partidoTorneo)
        {

            try
            {
                if (partidoTorneo == null) throw new Exception("No hay fixture para crear");

                string token = _usuarioServicio.ObtenerUsuarioLogueado().Token;
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.PostAsJsonAsync($"api/Fixture/Crear", partidoTorneo);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var partidos = JsonConvert.DeserializeObject<List<PartidoDTO>>(content);
                    return partidos;
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
