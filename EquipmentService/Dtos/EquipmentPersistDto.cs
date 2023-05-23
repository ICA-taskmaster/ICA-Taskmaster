using System.ComponentModel.DataAnnotations;

namespace EquipmentService.Dtos;

public class EquipmentPersistDto {
    [Required]
    public string item { get; init; }
    [Required]
    public string description { get; init; }
    [Required]
    public string status { get; init; }
}