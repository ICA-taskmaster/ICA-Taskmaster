using AgentService.Dtos;
using AgentService.Models;
using AutoMapper;

namespace AgentService.Profiles;

public class AgentsProfile : Profile {
    public AgentsProfile() {
        // Source -> Target
        CreateMap<Agent, AgentFetchDto>();
        CreateMap<AgentPersistDto, Agent>();
        CreateMap<AgentFetchDto, AgentPublishDto>();
        CreateMap<Agent, GrpcAgentModel>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.id))
            .ForMember(dest => dest.CodeName, opt => opt.MapFrom(src => src.codeName))
            .ForMember(dest => dest.SecurityClearance, opt => opt.MapFrom(src => src.securityClearance));
    }
}