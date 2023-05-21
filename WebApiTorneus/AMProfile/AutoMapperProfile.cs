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
        }
    }
}
