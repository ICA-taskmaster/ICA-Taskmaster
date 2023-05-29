using AgentService.Data;
using AutoMapper;
using Grpc.Core;

namespace AgentService.SyncDataServices.Grpc;

public class GrpcAgentService : GrpcAgent.GrpcAgentBase {
    private readonly IAgentRepository repository;
    private readonly IMapper mapper;

    public GrpcAgentService(IAgentRepository repository, IMapper mapper)
            => (this.repository, this.mapper) = (repository, mapper);
    
    public override Task<AgentResponse> getAllAgents(getAllRequest request, ServerCallContext context) {
        var response = new AgentResponse();
        var agents = repository.getAll();

        foreach (var agent in agents) 
            response.Agents.Add(mapper.Map<GrpcAgentModel>(agent));
        
        
        return Task.FromResult(response);
    }
}