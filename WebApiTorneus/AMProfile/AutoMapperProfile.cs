using AutoMapper;
using BDTorneus;
using Negocio.DTOs;

namespace WebApiTorneus.AMProfile
{
    public class AutoMapperProfile :  Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<RegistroDTO, Usuario>();
            CreateMap<Usuario, UsuarioLogueado>();
            CreateMap<Usuario, UsuarioDTO>().ReverseMap();
            CreateMap<Torneo, TorneoDTO>().ReverseMap();
            CreateMap<Torneo, TorneoCreacionDTO>().ReverseMap();
            CreateMap<Torneo, TorneoActualizacionDTO>().ReverseMap();
            CreateMap<Equipo, EquipoDTO>().ReverseMap();
            CreateMap<Partido, PartidoDTO>().ReverseMap();
            CreateMap<Inscripcion, InscripcionDTO>().ReverseMap();
            CreateMap<Jugador, JugadorDTO>().ReverseMap();
        }
    }
}
