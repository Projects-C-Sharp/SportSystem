using SportSistem.Models.Entities;
using SportSistem.Models.Enums;
using SportSistem.Responses;

namespace SportSistem.Services.Interfaces;

public interface ISpaceService
{
    Task<List<Space>> GetAllAsync();
    Task<List<Space>> GetByTypeAsync(SpaceType spaceType);
    Task<Space?> GetByIdAsync(int id);
    Task<ServiceResponse<Space>> CreateAsync(Space space);
    Task<ServiceResponse<Space>> UpdateAsync(Space space);
    Task<ServiceResponse<Space>> DeleteAsync(int id);
}