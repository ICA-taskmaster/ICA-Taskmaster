using AgentService.Models;

namespace AgentService.Data;

public interface IAgentRepository {
    bool saveChanges();

    IEnumerable<Agent> getAll();
    Agent getById(int id);
    Agent delete(int id);
    Agent create(Agent agent);
}