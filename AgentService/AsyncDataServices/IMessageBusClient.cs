using AgentService.Dtos;

namespace AgentService.AsyncDataServices;

public interface IMessageBusClient {
    void publishNewAgent(AgentPublishDto agentPublishDto);
}