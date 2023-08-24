using Blazored.LocalStorage;
using BlazorTorneusClient.Servicios;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor;
using MudBlazor.Services;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using TorneusClienteWeb;
using TorneusClienteWeb.Servicios;
using TorneusClienteWeb.Servicios_de_Datos;
using System.Text.Json.Serialization;
using System.Text.Json;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Tewr.Blazor.FileReader;

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

builder.Services.AddOptions();
builder.Services.AddScoped(sp =>
{
    var options = new JsonSerializerOptions
    {
        ReferenceHandler = ReferenceHandler.Preserve,
        // Otras opciones de configuración si es necesario
    };

    return options;
});

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

builder.Services.AddSingleton<HubConnection>(sp =>
{
    var navigationManager = sp.GetRequiredService<NavigationManager>();
    return new HubConnectionBuilder()
      .WithUrl(navigationManager.ToAbsoluteUri($"{builder.Configuration[cadenaConexionWebApi]}torneushubs"))
      .WithAutomaticReconnect()
      .Build();
});

builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddFileReaderService(options => options.UseWasmSharedBuffer = true);


builder.Services.AddScoped<ServicioMenu>();

builder.Services.AddScoped<UsuarioServicio>();
builder.Services.AddScoped<UsuarioServicioDatos>();

builder.Services.AddScoped<TorneoServicio>();
builder.Services.AddScoped<TorneoServicioDatos>();

builder.Services.AddScoped<ImagenServicio>();
builder.Services.AddScoped<ImagenServicioDatos>();

builder.Services.AddScoped<NotificacionesSignalRService>();

builder.Services.AddScoped<EquipoServicio>();
builder.Services.AddScoped<EquipoServicioDatos>();

builder.Services.AddScoped<JugadorServicio>();
builder.Services.AddScoped<JugadorServicioDatos>();

builder.Services.AddScoped<InscripcionServicio>();
builder.Services.AddScoped<InscripcionServicioDatos>();

builder.Services.AddScoped<MedioPagoServicio>();
builder.Services.AddScoped<MedioPagoServicioDatos>();

builder.Services.AddScoped<FixtureServicio>();
builder.Services.AddScoped<FixtureServicioDatos>();

builder.Services.AddScoped<AutPlanilleroServicio>();
builder.Services.AddScoped<AutPlanilleroServicioDatos>();

builder.Services.AddScoped<NotificacionServicio>();
builder.Services.AddScoped<NotificacionServicioDatos>();


await builder.Build().RunAsync();
