using System.ComponentModel.DataAnnotations;

namespace EquipmentService.Models;

public class Agent {
    [Key]
    [Required]
    public int id { get; set; }
    [Required]
    public int externalId { get; set; }
    [Required]
    public string codeName { get; set; }
    [Required]
    public string securityClearance { get; set; }
    public ICollection<Equipment> equipments { get; set; } = new List<Equipment>();
}