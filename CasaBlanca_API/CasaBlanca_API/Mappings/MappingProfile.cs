using AutoMapper;
using CasaBlanca_API.Models;
using CasaBlanca_API.Models.DTO;
using CasaBlanca_API.Models.DTO.Casas;
using CasaBlanca_API.Models.DTO.Usuario;

namespace CasaBlanca_API.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UsuarioRequest, ListaCasasDTO>();
            CreateMap<ListaCasasDTO, CasaResponse>();
        }
    }
}
