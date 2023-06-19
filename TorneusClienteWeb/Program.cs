using Blazored.LocalStorage;
using BlazorTorneusClient.Servicios;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor.Services;
using TorneusClienteWeb;
using TorneusClienteWeb.Servicios;
using TorneusClienteWeb.Servicios_de_Datos;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

string cadenaConexionWebApi;
#if DEBUG
cadenaConexionWebApi = "LocalWebApi";
#else
cadenaConexionWebApi = "ProduccionWebApi";
#endif

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.Configuration[cadenaConexionWebApi]) });


builder.Services.AddMudServices();
builder.Services.AddBlazoredLocalStorage();

builder.Services.AddScoped<ServicioMenu>();
builder.Services.AddScoped<UsuarioServicio>();
builder.Services.AddScoped<UsuarioServicioDatos>();




await builder.Build().RunAsync();
