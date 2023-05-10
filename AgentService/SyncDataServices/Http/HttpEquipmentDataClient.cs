using System.Text;
using System.Text.Json;
using AgentService.Dtos;

namespace AgentService.SyncDataServices.Http;

public class HttpEquipmentDataClient : IEquipmentDataClient {
    private readonly HttpClient httpClient;
    private readonly IConfiguration configuration;

    public HttpEquipmentDataClient(HttpClient httpClient, IConfiguration configuration) =>
    (this.httpClient, this.configuration) = (httpClient, configuration);
    
    public async Task sendAgentsToEquipmentService(AgentFetchDto agents) {
        var httpContent = new StringContent(
            JsonSerializer.Serialize(agents),
            Encoding.UTF8,
            "application/json"
        );
        var response = await httpClient.PostAsync($"{configuration["EquipmentService:Url"]}/api/c/agents/", httpContent);
        Console.WriteLine(response.IsSuccessStatusCode
            ? "--> Sync POST to EquipmentService was OK!"
            : "--> Sync POST to EquipmentService was NOT OK!");
    }
}