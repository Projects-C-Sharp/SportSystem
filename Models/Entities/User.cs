using System.ComponentModel.DataAnnotations;

namespace SportSistem.Models.Entities;

public class User
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Name is required.")]
    [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Identification is required.")]
    [StringLength(20, ErrorMessage = "Identification cannot exceed 20 characters.")]
    public string Identification { get; set; }

    [Required(ErrorMessage = "Phone is required.")]
    [StringLength(15, ErrorMessage = "Phone cannot exceed 15 characters.")]
    public string Phone { get; set; }

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email address.")]
    public string Email { get; set; }
}