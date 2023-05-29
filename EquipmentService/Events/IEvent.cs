namespace EquipmentService.Events;

public interface IEvent {
    void handleEvent(string message);
}