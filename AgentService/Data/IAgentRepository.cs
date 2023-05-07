using AgentService.Models;

namespace AgentService.Data;

public interface IAgentRepository {
    bool saveChanges();

    IEnumerable<Agent> getAll();
    Agent getById(ulong id);
    Agent delete(ulong id);
    Agent create(Agent agent);
}