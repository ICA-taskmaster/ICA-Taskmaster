﻿using AgentService.Dtos;
using AgentService.Models;
using AutoMapper;

namespace AgentService.Profiles;

public class AgentsProfile : Profile {
    public AgentsProfile() {
        // Source -> Target
        CreateMap<Agent, AgentFetchDto>();
        CreateMap<AgentPersistDto, Agent>();
        CreateMap<AgentFetchDto, AgentPublishDto>();
    }
}