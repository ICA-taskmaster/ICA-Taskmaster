using AutoMapper;
using EquipmentService.Data;
using EquipmentService.Dtos;
using EquipmentService.Models;
using Microsoft.AspNetCore.Mvc;

namespace EquipmentService.Controllers;

[Route("api/c/agents/{agentId}/[controller]")]
[ApiController]
public class EquipmentsController : ControllerBase {
    private readonly IEquipmentRespository repository;
    private readonly IMapper mapper;

    public EquipmentsController(IEquipmentRespository repository, IMapper mapper) 
        => (this.repository, this.mapper) = (repository, mapper);
    
    // GET api/c/agents/{agentId}/equipments
    [HttpGet]
    public ActionResult<IEnumerable<EquipmentFetchDto>> getEquipmentsForAgent(int agentId) {
        Console.WriteLine($"--> Getting equipments for agent id [{agentId}] from equipment service");
       
        if (!repository.agentExists(agentId))
            return NotFound();
        
        var equipments = repository.getEquipmentsForAgent(agentId);
        return Ok(mapper.Map<IEnumerable<EquipmentFetchDto>>(equipments));
    }
    
    // GET api/c/agents/{agentId}/equipments/{equipmentId}
    [HttpGet("{equipmentId}", Name = "getEquipmentForAgent")]
    public ActionResult<EquipmentFetchDto> getEquipmentForAgent(int agentId, int equipmentId) {
        Console.WriteLine($"--> Getting equipment id [{equipmentId}] for agent id [{agentId}] from equipment service");
       
        if (!repository.agentExists(agentId))
            return NotFound();

        var equipment = repository.getById(agentId, equipmentId);
        
        if (equipment == null)
            return NotFound();
        
        return Ok(mapper.Map<EquipmentFetchDto>(equipment));
    }
    
    // POST api/c/agents/{agentId}/equipments
    [HttpPost]
    public ActionResult<EquipmentFetchDto> createEquipmentForAgent(int agentId, EquipmentPersistDto equipmentPersistDto) {
        Console.WriteLine($"--> Creating equipment for agent id [{agentId}] from equipment service");
       
        if (!repository.agentExists(agentId))
            return NotFound();

        var equipment = mapper.Map<Equipment>(equipmentPersistDto);
        repository.create(agentId, equipment);
        repository.saveChanges();
        
        var equipmentFetchDto = mapper.Map<EquipmentFetchDto>(equipment);
        return CreatedAtRoute(nameof(getEquipmentForAgent), 
            new { agentId, equipmentId = equipmentFetchDto.id }, equipmentFetchDto);
    }
}