using AutoMapper;
using EventManagement.DTOs;
using EventManagement.Models;

namespace EventManagement.Mappings
{
    public class SectionMappingProfile : Profile
    {
        public SectionMappingProfile()
        {
            // Entity → DTO (GET)
            CreateMap<Section, SectionDto>()
                .ForMember(dest => dest.name, opt => opt.MapFrom(src => src.Name));

            // DTO → Entity (POST / PUT)
            CreateMap<SectionDto, Section>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.name));
        }
    }
}
