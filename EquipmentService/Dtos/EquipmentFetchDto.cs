namespace EquipmentService.Dtos;

public record EquipmentFetchDto(
    int id,
    string item,
    string description,
    string status,
    string imageUrl,
    int agentId
);