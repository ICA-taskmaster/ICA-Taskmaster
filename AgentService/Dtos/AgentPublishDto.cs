namespace AgentService.Dtos;

public class AgentPublishDto {
    public int id { get; set; }
    public string codeName { get; set; }
    public string securityClearance { get; set; }
    public string eventMq { get; set; }
}