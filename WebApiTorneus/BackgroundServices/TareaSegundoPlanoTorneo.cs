using BDTorneus;
using Negocio;
using Negocio.DTOs;
using System.IO;
using System.Reflection.Metadata;

namespace WebApiTorneus.BackgroundServices
{
    public class TareaSegundoPlanoTorneo : IHostedService, IDisposable
    {
        private Timer _timer;
        private List<TorneoInscripcionAbiertaDTO> torneos = new List<TorneoInscripcionAbiertaDTO>();
        private readonly IServiceScopeFactory scopeFactory;
        public TareaSegundoPlanoTorneo(IServiceScopeFactory scopeFactory)
        {
            this.scopeFactory = scopeFactory;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
           var tareaInicializacion = Task.Run(() => InicializarListado());
           tareaInicializacion.Wait();

          _timer = new Timer(VerificarFechaTorneo,null,TimeSpan.Zero,TimeSpan.FromMinutes(30));

           return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
           _timer?.Change(Timeout.Infinite,0);
            return Task.CompletedTask;
        }


        private async void VerificarFechaTorneo(object state)
        {
            if (torneos.Count > 0)
            {
                using (var scope = scopeFactory.CreateScope())
                {
                    var fechaHoy = DateTime.Now;
                    TimeSpan horaLimite = new TimeSpan(17, 0, 0);


                    foreach (var torneo in torneos)
                    {
                        if (torneo.FechaComienzo.Date.AddDays(-1) == fechaHoy.Date && fechaHoy.TimeOfDay > horaLimite)
                        {

                            var torneoService = scope.ServiceProvider.GetRequiredService<TorneoService>();
                            bool resultado = await torneoService.CerrarInscripciones(torneo);
                            if (resultado) torneos.Remove(torneo);
                        }
                    }
                }
            }
        }


        public void AgregarTorneo(TorneoInscripcionAbiertaDTO torneo)
        {
            torneos.Add(torneo);
        }

        public async Task InicializarListado()
        {
            using (var scope = scopeFactory.CreateScope())
            {
                if (torneos.Count == 0)
                {
                    var torneoService = scope.ServiceProvider.GetRequiredService<TorneoService>();
                    torneos = await torneoService.ListadoInscripcionesAbierta();
                }
               
            }
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }


    }
}
