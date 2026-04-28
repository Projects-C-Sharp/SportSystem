using Microsoft.EntityFrameworkCore;
using SportSistem.Data;
using SportSistem.Models.Entities;
using SportSistem.Responses;
using SportSistem.Services.Interfaces;

namespace SportSistem.Services;

public class UserService :  IUserService
{
    private readonly SportDbContext _context;
    private readonly IEmailService _emailService;

    public UserService(SportDbContext context, IEmailService emailService)
    {
        _context = context;
        _emailService = emailService;
    }


    public async Task<List<User>> GetAllAsync() =>
        await _context.Users.OrderBy(o => o.Name).ToListAsync();

    public async Task<User?> GetByIdAsync(int id) =>
        await _context.Users.FirstOrDefaultAsync(o => o.Id == id);

    public async Task<ServiceResponse<User>> CreateAsync(User user)
    {
        try
        {
            var userIdentification = await _context.Users.AnyAsync(o => o.Identification == user.Identification);
            if (userIdentification)
            {
                return new ServiceResponse<User>()
                {
                    Success = false,
                    Message = "An User with that Identification already exists."
                };
            }
                

            var userEmail = await _context.Users.AnyAsync(o => o.Email == user.Email);
            if (userEmail) return new ServiceResponse<User>() { 
                Success = false,
                Message = "An User with that email already exists."
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            
            // Send email notification
            try
            {

                var subject = "User Created Successfully";

                var content = string.Empty;
                content += EmailTemplate.BuildDetailItem("User", user.Name);
                content += EmailTemplate.BuildDetailItem("Identification", user.Identification);
                content += EmailTemplate.BuildDetailItem("Phone", user.Phone);
                content += EmailTemplate.BuildDetailItem("Email", user.Email);

                var body = EmailTemplate.BuildEmailHtml(
                    "Nuevo usuario registrado",
                    "Se ha creado un nuevo usuario en SportSystem con los datos a continuación.",
                    content);

                await _emailService.SendEmailAsync("ismaelvascog@gmail.com", $"ADMIN - {subject}", body);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email: {ex.Message}");
            }
            
            return new ServiceResponse<User>()
            {
                Success = true,
                Message = "User Created Correctly",
                Data = user
            };
        }
        catch (Exception ex)
        {
            return new ServiceResponse<User>() { 
                Success =false, Message = $"Error creating user: {ex.Message}"
            };
        }
    }

    public async Task<ServiceResponse<User>> UpdateAsync(User user)
    {
        try
        {
            var userIdentification = await _context.Users
                .AnyAsync(o => o.Identification == user.Identification && o.Id != user.Id);
            if (userIdentification) return  new ServiceResponse<User>()
                { 
                    Success = false, 
                    Message = "An User with that document already exists."
                };

            var userEmail = await _context.Users
                .AnyAsync(o => o.Email == user.Email && o.Id != user.Id);
            if (userEmail)
                return new ServiceResponse<User>()
                {
                    Success = false, Message = "An User with that email already exists."
                };

            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return new ServiceResponse<User>()
            {
                Success = true,
                Message = "User Updated Correctly",
                Data = user
            };
        }
        catch (Exception ex)
        {
            return new ServiceResponse<User>() { 
                Success = false, 
                Message = $"Error updating owner: {ex.Message}"
            };
        }
    }

    public async Task<ServiceResponse<User>> DeleteAsync(int id)
    {
        try
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
                return new ServiceResponse<User>()
                {
                    Success = false,
                    Message = "User not found."
                };

            var hasReservations = await _context.Reservations
                .AnyAsync(r => r.UserId == id);

            if (hasReservations)
                return new ServiceResponse<User>()
                {
                    Success = false,
                    Message = "Cannot delete user with registered reservations."
                };

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return new ServiceResponse<User>()
            {
                Success = true,
                Message = "User deleted successfully.",
                Data = user
            };
        }
        catch (Exception ex)
        {
            return new ServiceResponse<User>()
            {
                Success = false,
                Message = $"Error deleting user: {ex.Message}"
            };
        }
    }
}