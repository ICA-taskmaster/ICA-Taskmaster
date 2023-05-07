using System.ComponentModel.DataAnnotations;

namespace AgentService.Dtos;

public record AgentPersistDto {
    [Required]
    public string RealName { get; init; }
    [Required]
    public string CodeName { get; init; }
    public string BurnerPhone { get; init; }
    public string SecurityClearance { get; init; }
}