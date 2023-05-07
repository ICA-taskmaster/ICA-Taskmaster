namespace AgentService.Dtos;

public record AgentFetchDto(
    ulong id,
    string realName,
    string codeName,
    string burnerPhone,
    string securityClearance
    );