namespace EquipmentService.Dtos;

public class GenericEventDto {
    public string eventType { get; set; }
    public string eventMq { get; set; }
    public string eventPayload { get; set; }
}