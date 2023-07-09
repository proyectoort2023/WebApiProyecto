using AutoMapper;
using DTOs_Compartidos.DTOs;
using Negocio.DTOs;

namespace TorneusClienteWeb.Modelos
{
    public class ProfileAutomapper : Profile
    {
        public ProfileAutomapper()
        {
            CreateMap<TorneoDTO, TorneoDTO>().ReverseMap();
            CreateMap<TorneoDTO, TorneoActualizacionDTO>().ReverseMap();
            CreateMap<EquipoDTO, EquipoCreacionDTO>().ReverseMap();
        }
       
    }
}
