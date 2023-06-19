using System.ComponentModel.DataAnnotations;

namespace AgentService.Models;

public class Agent {
    [Key]
    [Required]
    public int id { get; set; }
    [Required]
    public string realName { get; set; }
    [Required]
    public string codeName { get; set; }
    public string burnerPhone { get; set; }
    public string securityClearance { get; set; }
} 