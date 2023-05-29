using AutoMapper;
using EquipmentService.Dtos;
using EquipmentService.Models;

namespace EquipmentService.Profiles;

public class EquipmentsProfile : Profile {
    public EquipmentsProfile() {
        // Source -> Target/destination
        CreateMap<Agent, AgentFetchDto>();
        CreateMap<Equipment, EquipmentFetchDto>();
        CreateMap<EquipmentPersistDto, Equipment>();
        CreateMap<AgentPublishedDto, Agent>()
            .ForMember(dest => dest.externalId, opt => opt.MapFrom(src => src.id));

    }
}