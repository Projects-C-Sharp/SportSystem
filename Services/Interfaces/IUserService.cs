using SportSistem.Models.Entities;
using SportSistem.Responses;

namespace SportSistem.Services.Interfaces;

public interface IUserService
{
    Task<List<User>> GetAllAsync();
    
    Task<User> GetByIdAsync(int id);
    
    Task<ServiceResponse<User>> CreateAsync(User user);

    Task<ServiceResponse<User>> UpdateAsync(User user);
    
    Task<ServiceResponse<User>> DeleteAsync(int id);

}