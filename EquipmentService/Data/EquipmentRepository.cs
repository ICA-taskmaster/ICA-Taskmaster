using EquipmentService.Models;

namespace EquipmentService.Data;

public class EquipmentRepository : IEquipmentRespository
{
    private readonly AppDbContext context;
    
    public EquipmentRepository(AppDbContext context) {
        this.context = context;
    }

    public IEnumerable<Agent> getAll() 
        => context.agents.ToList();

    public Agent create(Agent agent) {
        if (agent == null) 
            throw new ArgumentNullException(nameof(agent));
        
        context.agents.Add(agent);
        return agent;
    }

    public bool agentExists(int id) 
        => context.agents.Any(agent => agent.id == id);

    public Equipment getById(int agentId, int equipmentId)
        => context.equipments
            .FirstOrDefault(equipment => equipment.agentId == agentId && equipment.id == equipmentId);

    public IEnumerable<Equipment> getEquipmentsForAgent(int agentId) 
        => context.equipments
            .Where(equipment => equipment.agentId == agentId)
            .OrderBy(equipment => equipment.agent.name);
    
    public Equipment create(int agentId, Equipment equipment) {
        if (equipment == null) 
            throw new ArgumentNullException(nameof(equipment));
        
        equipment.agentId = agentId;
        context.equipments.Add(equipment);
        return equipment;
    }

    public Equipment delete(int id) {
        var equipment = getById(id, id);
        context.equipments.Remove(equipment);
        return equipment;
    }
    public bool saveChanges() => context.SaveChanges() >= 0;
}
