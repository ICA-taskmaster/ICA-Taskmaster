using AutoMapper;
using Azure.Storage.Blobs;
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
    private readonly IConfiguration configuration;

    public EquipmentsController(IEquipmentRespository repository, IMapper mapper, IConfiguration configuration) 
        => (this.repository, this.mapper, this.configuration) = (repository, mapper, configuration);
    
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
    public ActionResult<EquipmentFetchDto> createEquipmentForAgent(int agentId, [FromForm] EquipmentPersistDto equipmentPersistDto) {
        Console.WriteLine($"--> Creating equipment for agent id [{agentId}] from equipment service");
        
        if (!repository.agentExists(agentId))
            return NotFound();

        var equipment = mapper.Map<Equipment>(equipmentPersistDto);

        if (equipmentPersistDto.image != null) {
            Console.WriteLine("--> Uploading image to Azure Blob Storage");
            string connectionString = configuration["ConnectionStrings:StorageConnection"];
            string containerName = configuration["storageContainerName"];
            string imageName = Guid.NewGuid().ToString(); 
            var imageUrl = uploadImageToAzure(equipmentPersistDto.image.OpenReadStream(), connectionString, containerName, imageName); 
            
            equipment.imageUrl = imageUrl; 
        }

        repository.create(agentId, equipment);
        repository.saveChanges();
        
        var equipmentFetchDto = mapper.Map<EquipmentFetchDto>(equipment);
        return CreatedAtRoute(nameof(getEquipmentForAgent), 
            new { agentId, equipmentId = equipmentFetchDto.id }, equipmentFetchDto);
    }
    
    private static string uploadImageToAzure(Stream imageStream, string connectionString, string containerName, string imageName) {
        BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);
        BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);

        BlobClient blobClient = containerClient.GetBlobClient(imageName);
        blobClient.Upload(imageStream, true);

        return blobClient.Uri.ToString();
    }
}