using Microsoft.EntityFrameworkCore;
using SportSistem.Data;
using SportSistem.Models.Entities;
using SportSistem.Models.Enums;
using SportSistem.Responses;
using SportSistem.Services.Interfaces;

namespace SportSistem.Services;

public class ReservationService : IReservationService
{
    private readonly SportDbContext _context;
    private readonly IEmailService _emailService;

    public ReservationService(SportDbContext context, IEmailService emailService)
    {
        _context = context;
        _emailService = emailService;
    }

    public async Task<List<Reservation>> GetAllAsync() =>
        await _context.Reservations
            .Include(r => r.User)
            .Include(r => r.Space)
            .OrderBy(r => r.Date)
            .ThenBy(r => r.StartTime)
            .ToListAsync();

    public async Task<List<Reservation>> GetByUserAsync(int userId) =>
        await _context.Reservations
            .Where(r => r.UserId == userId)
            .Include(r => r.User)
            .Include(r => r.Space)
            .OrderBy(r => r.Date)
            .ThenBy(r => r.StartTime)
            .ToListAsync();

    public async Task<List<Reservation>> GetBySpaceAsync(int spaceId) =>
        await _context.Reservations
            .Where(r => r.SpaceId == spaceId)
            .Include(r => r.User)
            .Include(r => r.Space)
            .OrderBy(r => r.Date)
            .ThenBy(r => r.StartTime)
            .ToListAsync();

    public async Task<Reservation?> GetByIdAsync(int id) =>
        await _context.Reservations
            .Include(r => r.User)
            .Include(r => r.Space)
            .FirstOrDefaultAsync(r => r.Id == id);

    public async Task<ServiceResponse<Reservation>> CreateAsync(Reservation reservation)
    {
        try
        {
            // Validations
            if (reservation.EndTime <= reservation.StartTime)
            {
                return new ServiceResponse<Reservation>()
                {
                    Success = false,
                    Message = "End time must be after start time."
                };
            }

            var now = DateTime.Now;
            var reservationDateTime = reservation.Date.Date + reservation.StartTime.TimeOfDay;
            if (reservationDateTime < now)
            {
                return new ServiceResponse<Reservation>()
                {
                    Success = false,
                    Message = "Cannot create reservations in the past."
                };
            }

            // Check user conflicts
            var userConflicts = await _context.Reservations
                .Where(r => r.UserId == reservation.UserId && r.State == StateReservation.Scheduled)
                .AnyAsync(r =>
                    r.Date == reservation.Date &&
                    ((reservation.StartTime >= r.StartTime && reservation.StartTime < r.EndTime) ||
                     (reservation.EndTime > r.StartTime && reservation.EndTime <= r.EndTime) ||
                     (reservation.StartTime <= r.StartTime && reservation.EndTime >= r.EndTime))
                );
            if (userConflicts)
            {
                return new ServiceResponse<Reservation>()
                {
                    Success = false,
                    Message = "User already has a reservation in this time slot."
                };
            }

            // Check space conflicts
            var spaceConflicts = await _context.Reservations
                .Where(r => r.SpaceId == reservation.SpaceId && r.State == StateReservation.Scheduled)
                .AnyAsync(r =>
                    r.Date == reservation.Date &&
                    ((reservation.StartTime >= r.StartTime && reservation.StartTime < r.EndTime) ||
                     (reservation.EndTime > r.StartTime && reservation.EndTime <= r.EndTime) ||
                     (reservation.StartTime <= r.StartTime && reservation.EndTime >= r.EndTime))
                );
            if (spaceConflicts)
            {
                return new ServiceResponse<Reservation>()
                {
                    Success = false,
                    Message = "Space is already reserved in this time slot."
                };
            }

            reservation.State = StateReservation.Scheduled;
            _context.Reservations.Add(reservation);
            await _context.SaveChangesAsync();

            // Send email notification
            try
            {
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Id == reservation.UserId);

                var space = await _context.Spaces
                    .FirstOrDefaultAsync(s => s.Id == reservation.SpaceId);

                var subject = "Reservation Confirmed";

                var content = string.Empty;
                content += EmailTemplate.BuildDetailItem("User", user?.Name ?? "N/A");
                content += EmailTemplate.BuildDetailItem("Space", space?.Name ?? "N/A");
                content += EmailTemplate.BuildDetailItem("Date", reservation.Date.ToShortDateString());
                content += EmailTemplate.BuildDetailItem("Start Time", reservation.StartTime.ToShortTimeString());
                content += EmailTemplate.BuildDetailItem("End Time", reservation.EndTime.ToShortTimeString());
                content += EmailTemplate.BuildDetailItem("State", reservation.State.ToString());

                var body = EmailTemplate.BuildEmailHtml(
                    "Reserva confirmada",
                    "Tu reserva ha sido registrada correctamente y se encuentra en el sistema.",
                    content);

                await _emailService.SendEmailAsync(reservation.User?.Email ?? string.Empty, $"{user?.Name?.ToUpperInvariant()} - {subject}", body);
                await _emailService.SendEmailAsync("ismaelvascog@gmail.com", $"ADMIN - {subject}", body);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email: {ex.Message}");
            }

            return new ServiceResponse<Reservation>()
            {
                Success = true,
                Message = "Reservation Created Correctly",
                Data = reservation
            };
        }
        catch (Exception ex)
        {
            return new ServiceResponse<Reservation>() { 
                Success = false, 
                Message = $"Error creating reservation: {ex.Message}"
            };
        }
    }

    public async Task<ServiceResponse<Reservation>> CancelAsync(int id)
    {
        try
        {
            var reservation = await GetByIdAsync(id);
            if (reservation == null)
                return new ServiceResponse<Reservation>()
                {
                    Success = false,
                    Message = "Reservation not found."
                };

            if (reservation.State == StateReservation.Cancelled)
                return new ServiceResponse<Reservation>()
                {
                    Success = false,
                    Message = "Reservation is already cancelled."
                };

            reservation.State = StateReservation.Cancelled;
            _context.Reservations.Update(reservation);
            await _context.SaveChangesAsync();
            
            // Send email notification
            try
            {
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Id == reservation.UserId);

                var space = await _context.Spaces
                    .FirstOrDefaultAsync(s => s.Id == reservation.SpaceId);

                var subject = "Reservation Cancelled";

                var content = string.Empty;
                content += EmailTemplate.BuildDetailItem("User", user?.Name ?? "N/A");
                content += EmailTemplate.BuildDetailItem("Space", space?.Name ?? "N/A");
                content += EmailTemplate.BuildDetailItem("Date", reservation.Date.ToShortDateString());
                content += EmailTemplate.BuildDetailItem("Start Time", reservation.StartTime.ToShortTimeString());
                content += EmailTemplate.BuildDetailItem("End Time", reservation.EndTime.ToShortTimeString());
                content += EmailTemplate.BuildDetailItem("State", reservation.State.ToString());

                var body = EmailTemplate.BuildEmailHtml(
                    "Reserva cancelada",
                    "La reserva ha sido cancelada y su estado ha cambiado en SportSistem.",
                    content);

                await _emailService.SendEmailAsync(reservation.User?.Email ?? string.Empty, subject, body);
                await _emailService.SendEmailAsync("ismaelvascog@gmail.com", subject, body);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email: {ex.Message}");
            }
            
            return new ServiceResponse<Reservation>()
            {
                Success = true,
                Message = "Reservation Cancelled Correctly",
                Data = reservation
            };
        }
        catch (Exception ex)
        {
            return new ServiceResponse<Reservation>() { 
                Success = false, 
                Message = $"Error cancelling reservation: {ex.Message}"
            };
        }
    }
}