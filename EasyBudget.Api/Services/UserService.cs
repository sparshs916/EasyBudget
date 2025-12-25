namespace EasyBudget.Api.Services;

using EasyBudget.Api.Data;
using EasyBudget.Api.DTO;
using EasyBudget.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

public sealed class UserService
{
    private readonly ApiDbContext _context;
    private readonly ILogger<UserService> _logger;
    private readonly ITellerService _tellerService;

    public UserService(ApiDbContext context, ILogger<UserService> logger, ITellerService tellerService)
    {
        _context = context;
        _logger = logger;
        _tellerService = tellerService;
    }


    public async Task<bool> CreateNewUserAsync(CreateUserRequestDto request)
    {
        try
        {
            var newUser = new User 
            { 
                Email = request.Email,
                Username = request.Username,
                Auth0Id = request.Auth0Id
            };

            _context.Users.Add(newUser);
            
            // Push to database
            await _context.SaveChangesAsync();
            _logger.LogInformation("Created new user with username {Username}", newUser.Username);                
            return true;
        }
        catch (Exception ex)
        {
            // Log the actual exception so you can debug it
            _logger.LogError(ex, "Error occurred while creating user in database.");
            return false;
        }
    }

    public async Task<User?> GetUserAsync(GetUserRequestDto request){
        try{
            var user = await _context.Users
        .FirstOrDefaultAsync(u => u.Auth0Id == request.Auth0Id);

            if(user is null){
                _logger.LogError("No User found with username {username}", request.Auth0Id);
                return null;

            }

            _logger.LogInformation("GET USER: Found user {Username}", user.Username);
            return user;
        } catch (Exception ex){
            _logger.LogError(ex, "Error occurred on user GET {exception}", ex.Message);
            return null;
        }
    }

}