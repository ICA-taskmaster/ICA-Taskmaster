namespace AgentService.Dtos;

public record AgentFetchDto(
    int id,
    string realName,
    string codeName,
    string burnerPhone,
    string securityClearance
    );