using AgentService.Models;

namespace AgentService.Data;

public class AgentRepository : IAgentRepository {
    private readonly AppDbContext context;

    public AgentRepository(AppDbContext context) {
        this.context = context;
    }

    public IEnumerable<Agent> getAll() => context.agents.ToList();
    
    public Agent getById(ulong id) => context.agents.FirstOrDefault(agent => agent.id == id) ?? throw new InvalidOperationException();
    
    public Agent delete(ulong id) {
        var agent = getById(id);
        context.agents.Remove(agent);
        return agent;
    }

    public Agent create(Agent agent) {
        if (agent == null) 
            throw new ArgumentNullException(nameof(agent));
        
        context.agents.Add(agent);
        return agent;
    }
    
    public bool saveChanges() => context.SaveChanges() >= 0;
}