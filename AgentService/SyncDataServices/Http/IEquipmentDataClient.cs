using AgentService.Dtos;

namespace AgentService.SyncDataServices.Http;

public interface IEquipmentDataClient {
    Task sendAgentsToEquipmentService(AgentFetchDto agents);
}