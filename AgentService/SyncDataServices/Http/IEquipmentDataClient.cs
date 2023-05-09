using AgentService.Dtos;

namespace AgentService.SyncDataServices.Http;

public interface IEquipmentDataClient
{
    Task SendAgentsToEquipmentService(IEnumerable<AgentFetchDto> agents);
}