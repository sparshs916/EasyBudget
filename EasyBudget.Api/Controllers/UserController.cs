namespace EasyBudget.Api.Controllers;

using Microsoft.AspNetCore.Mvc;
using EasyBudget.Api.DTO;
using EasyBudget.Api.Services;
using System.Security.Claims;
using EasyBudget.Api.Services.Interfaces;

[ApiController]
[Route("api/user")]
public class UserController(
    IUserService userService
) : ControllerBase
{
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