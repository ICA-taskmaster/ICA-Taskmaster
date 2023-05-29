using EquipmentService.Models;

namespace EquipmentService.Data;

public interface IEquipmentRespository {
    bool saveChanges();

    //Agents 
    IEnumerable<Agent> getAll();
    Agent create(Agent agent);
    bool agentExists(int id);
    bool externalAgentExists(int externalId);
    
    //Equipments
    Equipment getById(int agentId, int equipmentId);
    IEnumerable<Equipment> getEquipmentsForAgent(int agentId);
    Equipment create(int agentId, Equipment equipment);
    Equipment delete(int id);
} 