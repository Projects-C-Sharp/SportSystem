using System.ComponentModel.DataAnnotations;
using SportSistem.Models.Enums;

namespace SportSistem.Models.Entities;

public class Space
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Name is required.")]
    [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Space type is required.")]
    public SpaceType SpaceType { get; set; }

    [Required(ErrorMessage = "Capacity is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Capacity must be at least 1.")]
    public int Capacity { get; set; }
}