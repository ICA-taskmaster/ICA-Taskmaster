using System.ComponentModel.DataAnnotations;

namespace EquipmentService.Models;

public class Equipment {
    [Key]
    [Required]
    public int id { get; set; }
    [Required]
    public string item { get; set; }
    [Required]
    public string description { get; set; }
    [Required]
    public string status { get; set; }
    [Required]
    public byte[] image { get; set; }
    [Required]
    public int agentId { get; set; }
    
    public Agent agent { get; set; }
} 