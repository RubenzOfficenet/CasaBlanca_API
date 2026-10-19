using Core.Mappy.Interfaces;

namespace CasaBlanca_API.Models.DTO.Casa
{
    public class CasaMappingProfile : IMappingProfile
    {
        public void Configure(IMapper mapper)
        {
            mapper.CreateMap<Inmueble, CasaDto>(
                cfg => {
                    cfg.Map(dest => dest.Id, src => src.Id);
                    cfg.Map(dest => dest.numeroCasa, src => src.NumeroCasa);
                }
            );
        }
    }
}
