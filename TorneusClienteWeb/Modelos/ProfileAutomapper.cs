using AutoMapper;
using DTOs_Compartidos.DTOs;
using Negocio.DTOs;

namespace TorneusClienteWeb.Modelos
{
    public class ProfileAutomapper : Profile
    {
        public ProfileAutomapper()
        {
            CreateMap<TorneoDTO, TorneoDTO>();
            CreateMap<TorneoDTO, TorneoActualizacionDTO>().ReverseMap();
        }
       
    }
}
