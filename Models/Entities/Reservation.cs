using System.ComponentModel.DataAnnotations;
using SportSistem.Models.Enums;

namespace SportSistem.Models.Entities;

public class Reservation
{
    public int Id { get; set; }

    [Required(ErrorMessage = "User is required.")]
    public int? UserId { get; set; }
    public User? User { get; set; }

    [Required(ErrorMessage = "Space is required.")]
    public int? SpaceId { get; set; }
    public Space? Space { get; set; }

    [Required(ErrorMessage = "Date is required.")]
    [DataType(DataType.Date)]
    public DateTime Date { get; set; }

    [Required(ErrorMessage = "Start time is required.")]
    [DataType(DataType.Time)]
    public DateTime StartTime { get; set; }

    [Required(ErrorMessage = "End time is required.")]
    [DataType(DataType.Time)]
    public DateTime EndTime { get; set; }

    public StateReservation State { get; set; }
}