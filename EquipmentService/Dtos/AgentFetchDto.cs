namespace EquipmentService.Dtos;

public record AgentFetchDto(
    int id,
    string codeName,
    string securityClearance
);