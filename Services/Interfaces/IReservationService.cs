using SportSistem.Models.Entities;
using SportSistem.Responses;

namespace SportSistem.Services.Interfaces;

public interface IReservationService
{
    Task<List<Reservation>> GetAllAsync();
    Task<List<Reservation>> GetByUserAsync(int userId);
    Task<List<Reservation>> GetBySpaceAsync(int spaceId);
    Task<Reservation?> GetByIdAsync(int id);
    Task<ServiceResponse<Reservation>> CreateAsync(Reservation reservation);
    Task<ServiceResponse<Reservation>> CancelAsync(int id);
}