using Microsoft.EntityFrameworkCore;
using SportSistem.Data;
using SportSistem.Models.Entities;
using SportSistem.Models.Enums;
using SportSistem.Responses;
using SportSistem.Services.Interfaces;

namespace SportSistem.Services;

public class SpaceService : ISpaceService
{
    private readonly SportDbContext _context;
    private readonly IEmailService _emailService;

    public SpaceService(SportDbContext context, IEmailService emailService)
    {
        _context = context;
        _emailService = emailService;
    }

    public async Task<List<Space>> GetAllAsync() =>
        await _context.Spaces.OrderBy(o => o.Name).ToListAsync();

    public async Task<List<Space>> GetByTypeAsync(SpaceType spaceType) =>
        await _context.Spaces.Where(s => s.SpaceType == spaceType).OrderBy(o => o.Name).ToListAsync();

    public async Task<Space?> GetByIdAsync(int id) =>
        await _context.Spaces.FirstOrDefaultAsync(o => o.Id == id);

    public async Task<ServiceResponse<Space>> CreateAsync(Space space)
    {
        try
        {
            var spaceExists = await _context.Spaces.AnyAsync(o => o.Name == space.Name);
            if (spaceExists)
            {
                return new ServiceResponse<Space>()
                {
                    Success = false,
                    Message = "A Space with that name already exists."
                };
            }

            _context.Spaces.Add(space);
            await _context.SaveChangesAsync();
            
            // Send email notification
            try
            {

                var subject = "Space Created Successfully";

                var content = string.Empty;
                content += EmailTemplate.BuildDetailItem("Space", space.Name);
                content += EmailTemplate.BuildDetailItem("Type", space.SpaceType.ToString());
                content += EmailTemplate.BuildDetailItem("Capacity", space.Capacity.ToString());

                var body = EmailTemplate.BuildEmailHtml(
                    "Nuevo espacio registrado",
                    "Se ha agregado un nuevo espacio deportivo con los detalles a continuación.",
                    content);

                await _emailService.SendEmailAsync("ismaelvascog@gmail.com", $"ADMIN - {subject}", body);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email: {ex.Message}");
            }
            
            return new ServiceResponse<Space>()
            {
                Success = true,
                Message = "Space Created Correctly",
                Data = space
            };
        }
        catch (Exception ex)
        {
            return new ServiceResponse<Space>() { 
                Success = false, 
                Message = $"Error creating space: {ex.Message}"
            };
        }
    }

    public async Task<ServiceResponse<Space>> UpdateAsync(Space space)
    {
        try
        {
            var spaceExists = await _context.Spaces
                .AnyAsync(o => o.Name == space.Name && o.Id != space.Id);
            if (spaceExists) 
                return new ServiceResponse<Space>()
                { 
                    Success = false, 
                    Message = "A Space with that name already exists."
                };

            _context.Spaces.Update(space);
            await _context.SaveChangesAsync();
            return new ServiceResponse<Space>()
            {
                Success = true,
                Message = "Space Updated Correctly",
                Data = space
            };
        }
        catch (Exception ex)
        {
            return new ServiceResponse<Space>() { 
                Success = false, 
                Message = $"Error updating space: {ex.Message}"
            };
        }
    }

    public async Task<ServiceResponse<Space>> DeleteAsync(int id)
    {
        try
        {
            var space = await GetByIdAsync(id);
            if (space == null)
                return new ServiceResponse<Space>()
                {
                    Success = false,
                    Message = "Space not found."
                };

            _context.Spaces.Remove(space);
            await _context.SaveChangesAsync();
            return new ServiceResponse<Space>()
            {
                Success = true,
                Message = "Space Deleted Correctly"
            };
        }
        catch (Exception ex)
        {
            return new ServiceResponse<Space>() { 
                Success = false, 
                Message = $"Error deleting space: {ex.Message}"
            };
        }
    }
}