using AgentService.AsyncDataServices;
using AgentService.Data;
using AgentService.Dtos;
using AgentService.Models;
using AgentService.SyncDataServices.Http;
using AgentService.Validation;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Security.Application;

namespace AgentService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AgentsController : ControllerBase {
    private readonly IAgentRepository repository;
    private readonly IMapper mapper;
    private readonly IEquipmentDataClient equipmentDataClient;
    private readonly IMessageBusClient messageBusClient;
    private readonly ILogger<AgentsController> logger;

    public AgentsController(
        IAgentRepository repository, IMapper mapper, IEquipmentDataClient equipmentDataClient, IMessageBusClient messageBusClient, ILogger<AgentsController> logger
    ) => (
        this.repository, this.mapper, this.equipmentDataClient, this.messageBusClient, this.logger
    ) = (
        repository, mapper, equipmentDataClient, messageBusClient, logger
    );
    
    // GET api/agents
    [HttpGet]
    public ActionResult<IEnumerable<AgentFetchDto>> getAllAgents() {
        logger.LogInformation("Getting all agents...");

        var agents = repository.getAll();
        var agentFetchDtos = mapper.Map<IEnumerable<AgentFetchDto>>(agents);

        logger.LogInformation("Retrieved {Count} agents", agentFetchDtos.Count());

        return Ok(agentFetchDtos);
    }

    // GET api/agents/{id}
    [HttpGet("{id}", Name = "GetAgentById")]
    public ActionResult<AgentFetchDto> getAgentById(int id) {
        logger.LogInformation("Getting agent by ID: {Id}", id);

        var agent = repository.getById(id);

        if (agent != null) {
            var agentFetchDto = mapper.Map<AgentFetchDto>(agent);
            logger.LogInformation("Retrieved agent with ID {Id}", agentFetchDto.id);
            return Ok(agentFetchDto);
        }

        logger.LogWarning("Agent with ID {Id} was not found", id);

        return NotFound();
    }

    // POST api/agents
    [HttpPost]
    public async Task<ActionResult<AgentFetchDto>> createAgent(AgentPersistDto agentPersistDto) {
        logger.LogInformation("Creating agent...");
        var agent = mapper.Map<Agent>(agentPersistDto);
        bool isValidAgent = InputValidator.validateAgent(agent);
        
        if (!isValidAgent) {
            logger.LogWarning("Invalid agent");
            return BadRequest();
        }
        
        agent.realName = Encoder.HtmlEncode(agent.realName);
        agent.codeName = Encoder.HtmlEncode(agent.codeName);
        agent.burnerPhone = Encoder.HtmlEncode(agent.burnerPhone);

        repository.create(agent);
        repository.saveChanges();

        var agentFetchDto = mapper.Map<AgentFetchDto>(agent);

        // Send Sync Message
        try {
            await equipmentDataClient.sendAgentsToEquipmentService(agentFetchDto);
            logger.LogInformation("Synchronously sent agent to equipment service");
        }
        catch (Exception e) {
            logger.LogError(e, "Could not send agent synchronously: {ErrorMessage}", e.Message);
        }

        // Send Async Message
        try {
            var agentPublishDto = mapper.Map<AgentPublishDto>(agentFetchDto);
            agentPublishDto.eventMq = "Agent_Published";
            messageBusClient.publishNewAgent(agentPublishDto);
            logger.LogInformation("Asynchronously sent agent to message bus");
        }
        catch (Exception e) {
            logger.LogError(e, "Could not send agent asynchronously: {ErrorMessage}", e.Message);
        }

        logger.LogInformation("Created agent with ID {Id}", agentFetchDto.id);

        return CreatedAtRoute(nameof(getAgentById), new { agentFetchDto.id }, agentFetchDto);
    }
}