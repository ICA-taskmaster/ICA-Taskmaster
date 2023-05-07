using System.ComponentModel.DataAnnotations;

namespace AgentService.Dtos;

public class AgentPersistDto {
    [Required]
    public string realName { get; set; }
    [Required]
    public string codeName { get; set; }
    public string burnerPhone { get; set; }
    public string securityClearance { get; set; }
}