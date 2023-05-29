using System.Text.Json;
using AutoMapper;
using EquipmentService.Data;
using EquipmentService.Dtos;
using EquipmentService.Models;

namespace EquipmentService.Events;

public class Event : IEvent {
    private readonly IServiceScopeFactory scopeFactory;
    private readonly IMapper mapper;

    public Event(IServiceScopeFactory scopeFactory, IMapper mapper)
        => (this.scopeFactory, this.mapper) = (scopeFactory, mapper);
    
    public void handleEvent(string message) {
        Console.WriteLine($"--> Handling event: {message}");
        var eventType = determineEvent(message);
        switch (eventType) {
            case EventType.AgentPublished:
                handleAgentPublished(message);
                break;
            case EventType.Undetermined:
            default:
                Console.WriteLine($"--> Event type {eventType} not handled");
                break;
        }
    }

    private EventType determineEvent(string message) {
        Console.WriteLine("--> Determining event");
        var dto = JsonSerializer.Deserialize<genericEventDto>(message);
        return dto.eventType switch {
            "AgentPublished" => EventType.AgentPublished,
            _ => EventType.Undetermined
        };
    }
    
    private void handleAgentPublished(string message) {
        using var scope = scopeFactory.CreateScope();
        
        var repository = scope.ServiceProvider.GetRequiredService<IEquipmentRespository>();
        var agentPublishedDto = JsonSerializer.Deserialize<AgentPublishedDto>(message);

        try {
            var agent = mapper.Map<Agent>(agentPublishedDto);
            
            if (!repository.externalAgentExists(agent.externalId)) {
                repository.create(agent);
                repository.saveChanges();
            } else {
                Console.WriteLine($"--> Agent with externalId [{agent.externalId}] already exists");
            }
        } catch (Exception e) {
            Console.WriteLine($"--> Could not add Agent to DB: {e.Message}");
        }
    }
}

internal enum EventType {
    AgentPublished,
    Undetermined
}