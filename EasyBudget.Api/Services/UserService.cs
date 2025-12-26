namespace EasyBudget.Api.Services;

using EasyBudget.Api.Data;
using EasyBudget.Api.DTO;
using EasyBudget.Api.Models;
using EasyBudget.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

public sealed class UserService(
    ApiDbContext context, ILogger<UserService> logger
    ) : IUserService
{
    public async Task<bool> CreateNewUserAsync(string Auth0Id, CreateUserRequestDto request)
    {
        try
        {
            User? newUser = new User
            {
                Email = request.Email,
                Username = request.Username,
                Auth0Id = Auth0Id
            };

            context.Users.Add(newUser);

            // Push to database
            await context.SaveChangesAsync();
            logger.LogInformation("Created new user with username {Username}", newUser.Username);
            return true;
        }
        catch (Exception ex)
        {
            // Log the actual exception so you can debug it
            logger.LogError(ex, "Error occurred while creating user in database.");
            return false;
        }
    }

    public async Task<User?> GetUserAsync(string auth0Id)
    {
        try
        {
            User? user = await context.Users
                .FirstOrDefaultAsync(u => u.Auth0Id == auth0Id);

            if (user is null)
            {
                logger.LogError("No User found with Auth0Id");
                return null;
            }

            logger.LogInformation("GET USER: Found user {Username}", user.Username);
            return user;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred on user GET {exception}", ex.Message);
            return null;
        }
    }

}