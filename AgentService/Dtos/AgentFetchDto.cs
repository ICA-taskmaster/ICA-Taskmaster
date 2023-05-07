namespace AgentService.Dtos;

public class AgentFetchDto {
    public ulong id { get; set; }
    public string realName { get; set; }
    public string codeName { get; set; }
    public string burnerPhone { get; set; }
    public string securityClearance { get; set; }
}