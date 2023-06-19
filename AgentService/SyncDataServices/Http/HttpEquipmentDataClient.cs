using System.Text;
using System.Text.Json;
using AgentService.Dtos;

namespace AgentService.SyncDataServices.Http;

public class HttpEquipmentDataClient : IEquipmentDataClient {
    private readonly HttpClient httpClient;
    private readonly IConfiguration configuration;
    private readonly ILogger<HttpEquipmentDataClient> logger;

    public HttpEquipmentDataClient(HttpClient httpClient, IConfiguration configuration, ILogger<HttpEquipmentDataClient> logger) =>
    (this.httpClient, this.configuration, this.logger) = (httpClient, configuration, logger);
    
    public async Task sendAgentsToEquipmentService(AgentFetchDto agents) {
        var httpContent = new StringContent(
            JsonSerializer.Serialize(agents),
            Encoding.UTF8,
            "application/json"
        );
        var response = await httpClient.PostAsync($"{configuration["EquipmentService:Url"]}/api/c/agents/", httpContent);

        if (response.IsSuccessStatusCode) {
            logger.LogInformation("Sync POST to EquipmentService was OK!");
        } else {
            logger.LogError("Sync POST to EquipmentService was NOT OK!");
        }
        
    }
}