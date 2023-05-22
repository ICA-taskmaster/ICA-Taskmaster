using AgentService.Data;
using AgentService.Dtos;
using AgentService.Models;
using AgentService.SyncDataServices.Http;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace AgentService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AgentsController : ControllerBase {
    private readonly IAgentRepository repository;
    private readonly IMapper mapper;
    private readonly IEquipmentDataClient equipmentDataClient;

    public AgentsController(
        IAgentRepository repository, IMapper mapper, IEquipmentDataClient equipmentDataClient
        ) => (
        this.repository, this.mapper, this.equipmentDataClient
        ) = (
            repository, mapper, equipmentDataClient
        );
    
    // GET api/agents
    [HttpGet]
    public ActionResult<IEnumerable<AgentFetchDto>> getAllAgents() {
        Console.WriteLine("--> Getting Agents...");
        var agents = repository.getAll();
        return Ok(mapper.Map<IEnumerable<AgentFetchDto>>(agents));
    }
    
    // GET api/agents/{id}
    [HttpGet("{id}", Name = "GetAgentById")]
    public ActionResult<AgentFetchDto> getAgentById(int id) {
        Console.WriteLine("--> Getting Agent by id...");
        var agent = repository.getById(id);
        
        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
        if (agent != null) 
            return Ok(mapper.Map<AgentFetchDto>(agent));
        
        return NotFound();
    }
    
    // POST api/agents
    [HttpPost]
    public async Task<ActionResult<AgentFetchDto>> createAgent(AgentPersistDto agentPersistDto) {
        Console.WriteLine("--> Creating Agent...");
        var agent = mapper.Map<Agent>(agentPersistDto);
        repository.create(agent);
        repository.saveChanges();
        
        var agentFetchDto = mapper.Map<AgentFetchDto>(agent);
        
        try {
            await equipmentDataClient.sendAgentsToEquipmentService(agentFetchDto);
        }
        catch(Exception ex) {
            Console.WriteLine($"--> Could not send synchronously: {ex.Message}");
        }
        
        return CreatedAtRoute(nameof(getAgentById), new { agentFetchDto.id }, agentFetchDto);
    }
}