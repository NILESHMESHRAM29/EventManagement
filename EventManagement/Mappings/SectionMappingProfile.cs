using AutoMapper;

namespace EventManagement.Mappings
{
    public class SectionMappingProfile : Profile
    {
        public SectionMappingProfile()
        {
            CreateMap<DTOs.SectionDto, Models.Section>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.name));
        }
    }
}
