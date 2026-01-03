namespace EasyBudget.Api.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using EasyBudget.Api.DTO;
using EasyBudget.Api.Services;
using System.Security.Claims;
using System.Security.Cryptography;
using EasyBudget.Api.Services.Interfaces;

[ApiController]
[Route("api/user")]
public class UserController(
    IUserService userService,
    IConfiguration configuration,
    ILogger<UserController> logger
) : ControllerBase
{
    /// <summary>
    /// Webhook endpoint for Auth0 Post-Registration Action.
    /// This endpoint bypasses JWT auth and uses a shared secret instead.
    /// </summary>
    [HttpPost("webhook")]
    [AllowAnonymous]
    public async Task<IActionResult> CreateUserFromWebhook([FromBody] Auth0WebhookUserDto request)
    {
        logger.LogInformation("Webhook received for email: {Email}", request.Email);
        
        // Verify the webhook secret (constant-time comparison to prevent timing attacks)
        var providedSecret = Request.Headers["X-Auth0-Webhook-Secret"].FirstOrDefault()?.Trim();
        var expectedSecret = configuration["Auth0:WebhookSecret"]?.Trim();

        if (string.IsNullOrEmpty(providedSecret) || 
            string.IsNullOrEmpty(expectedSecret) ||
            !CryptographicOperations.FixedTimeEquals(
                System.Text.Encoding.UTF8.GetBytes(providedSecret),
                System.Text.Encoding.UTF8.GetBytes(expectedSecret)))
        {
            logger.LogWarning("Webhook called with invalid or missing secret");
            return Unauthorized("Invalid webhook secret");
        }

        // Check if user already exists
        var existingUser = await userService.GetUserAsync(request.Auth0Id);
        if (existingUser is not null)
        {
            logger.LogInformation("User {Auth0Id} already exists, skipping creation", request.Auth0Id);
            return Ok(new { Message = "User already exists" });
        }

        logger.LogInformation("User does not exist, creating new user...");

        // Create the user
        var createRequest = new CreateUserRequestDto(request.Username, request.Email);
        bool success = await userService.CreateNewUserAsync(request.Auth0Id, createRequest);

        if (!success)
        {
            logger.LogError("Failed to create user from webhook for {Auth0Id}", request.Auth0Id);
            return BadRequest("Could not create new user");
        }

        logger.LogInformation("=== USER CREATED SUCCESSFULLY: {Auth0Id} ===", request.Auth0Id);
        return Ok(new { Message = "User created!" });
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserRequestDto request)
    {
        var auth0Id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(auth0Id))
        {
            return Unauthorized("Could not determine user identity");
        }

        bool success = await userService.CreateNewUserAsync(auth0Id, request);

        if (!success)
        {
            return BadRequest("Could not create new user");
        }

        return Ok(new { Message = "User created!" });
    }

    [HttpGet]
    public async Task<IActionResult> GetUser()
    {
        var auth0Id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(auth0Id))
        {
            return Unauthorized("Could not determine user identity");
        }

        var user = await userService.GetUserAsync(auth0Id);
        if (user is null)
        {
            return NotFound("User not found");
        }
        return Ok(user);
    }

}