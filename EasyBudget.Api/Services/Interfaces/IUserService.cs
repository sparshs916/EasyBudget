namespace EasyBudget.Api.Services.Interfaces;  
using EasyBudget.Api.DTO;
using EasyBudget.Api.Models;

public interface IUserService
{
    Task<bool> CreateNewUserAsync(string auth0Id,CreateUserRequestDto request);
    Task<User?> GetUserAsync(string auth0Id);
}
