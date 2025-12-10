using AutoMapper;
using EventManagement.DTOs;
using EventManagement.Models;


namespace EventManagement.Mappings
{
    public class EventMappingProfile : Profile
    {
        public EventMappingProfile()
        {
            CreateMap<Event, EventResponseDto>();

            CreateMap<EventCreateDto, Event>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow));

            CreateMap<EventUpdateDto, Event>()
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow));
        }
    }
}
