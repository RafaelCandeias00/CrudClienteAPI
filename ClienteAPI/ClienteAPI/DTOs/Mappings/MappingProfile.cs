using AutoMapper;
using ClienteAPI.Models;

namespace ClienteAPI.DTOs.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            CreateMap<Cliente, ClienteDTO>().ReverseMap();
        }
    }
}
