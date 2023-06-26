using Blazored.LocalStorage;
using BlazorTorneusClient.Servicios;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor;
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


builder.Services.AddMudServices(config =>
{
    config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.TopCenter;

    config.SnackbarConfiguration.PreventDuplicates = false;
    config.SnackbarConfiguration.NewestOnTop = false;
    config.SnackbarConfiguration.ShowCloseIcon = false;
    config.SnackbarConfiguration.VisibleStateDuration = 2500;
    config.SnackbarConfiguration.HideTransitionDuration = 200;
    config.SnackbarConfiguration.ShowTransitionDuration = 200;
    config.SnackbarConfiguration.SnackbarVariant = Variant.Filled;
});
builder.Services.AddBlazoredLocalStorage();


builder.Services.AddScoped<ServicioMenu>();
builder.Services.AddScoped<UsuarioServicio>();
builder.Services.AddScoped<UsuarioServicioDatos>();

builder.Services.AddScoped<TorneoServicio>();
builder.Services.AddScoped<TorneoServicioDatos>();

builder.Services.AddScoped<ImagenServicio>();
builder.Services.AddScoped<ImagenServicioDatos>();



await builder.Build().RunAsync();
