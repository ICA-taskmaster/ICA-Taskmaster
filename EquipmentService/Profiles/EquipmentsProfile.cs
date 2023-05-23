using AutoMapper;
using EquipmentService.Dtos;
using EquipmentService.Models;

namespace EquipmentService.Profiles;

public class EquipmentsProfile : Profile {
    public EquipmentsProfile() {
        // Source -> Target
        CreateMap<Agent, AgentFetchDto>();
        CreateMap<Equipment, EquipmentFetchDto>();
        CreateMap<EquipmentPersistDto, Equipment>();
    }
}