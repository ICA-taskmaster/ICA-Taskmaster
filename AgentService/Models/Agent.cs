using System.ComponentModel.DataAnnotations;

namespace AgentService.Models;

public class Agent {
    [Key]
    [Required]
    public ulong id { get; set; }
    [Required]
    public string realName { get; set; }
    [Required]
    public string codeName { get; set; }
    public string burnerPhone { get; set; }
    public string securityClearance { get; set; }
    /*public List<Passport> passports { get; set; }
    public List<Skill> Skills { get; set; }
    public List<Equipment> Equipments { get; set; }*/
} 