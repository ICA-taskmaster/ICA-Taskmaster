using AutoMapper;
using EquipmentService.Data;
using EquipmentService.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace EquipmentService.Controllers;

[Route("api/c/[controller]")]
[ApiController]
public class AgentsController : ControllerBase {
    private readonly IEquipmentRespository repository;
    private readonly IMapper mapper;

    public AgentsController(IEquipmentRespository repository, IMapper mapper) 
        => (this.repository, this.mapper) = (repository, mapper);
    
    [HttpGet]
    public ActionResult<IEnumerable<AgentFetchDto>> getAgents() {
        Console.WriteLine("--> Getting agents from equipment service");
        var agents = repository.getAll();
        return Ok(mapper.Map<IEnumerable<AgentFetchDto>>(agents));
    }

    // POST api/c/agents/test
    [HttpPost]
    public ActionResult testInboundConnection() {
        Console.WriteLine("--> received inbound connection from POST equipment service");
        return Ok("--> inbound test of from agents controller");
    }
}