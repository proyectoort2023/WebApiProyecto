using AutoMapper;
using BDTorneus;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Negocio.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingTorneus
{
    public class BasePruebas
    {
        protected TorneoContext ConstruirContext(string nombreBD)
        {
            var optionsBuilder = new DbContextOptionsBuilder<TorneoContext>()
          .UseInMemoryDatabase(nombreBD)
            .Options;

            var dbContext = new TorneoContext(optionsBuilder);
            return dbContext;
        }

        protected IMapper ConfigurarAutoMapper()
        {
            var config = new MapperConfiguration(opciones =>
            {
                opciones.CreateMap<RegistroDTO, Usuario>();
                opciones.CreateMap<Usuario, UsuarioLogueado>();
                opciones.CreateMap<Usuario, UsuarioDTO>().ReverseMap();
                opciones.CreateMap<Torneo, TorneoDTO>().ReverseMap();
                opciones.CreateMap<Torneo, TorneoCreacionDTO>().ReverseMap();
                opciones.CreateMap<Equipo, EquipoDTO>().ReverseMap();
                opciones.CreateMap<Partido, PartidoDTO>().ReverseMap();
                opciones.CreateMap<Inscripcion, InscripcionDTO>().ReverseMap();
                opciones.CreateMap<Jugador, JugadorDTO>().ReverseMap();
            });
            return config.CreateMapper();
        }
        

        protected IConfiguration MiConfiguracionTest()
        {
            var configurcion = new ConfigurationBuilder()
           .AddInMemoryCollection(new[]
           {
                new KeyValuePair<string, string>("Jwt:SecretKey", "TuSecretKey"),
              
           })
           .Build();

            return configurcion;
        }

    }
}
