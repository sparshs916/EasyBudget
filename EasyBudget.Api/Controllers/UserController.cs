namespace EasyBudget.Api.Controllers;

using Microsoft.AspNetCore.Mvc;
using EasyBudget.Api.DTO;
using EasyBudget.Api.Services;

[ApiController]
[Route("api/user")] // This makes the URL: /api/User
public class UserController : ControllerBase
{
    private readonly UserService _userService;

    public UserController(UserService userService)
    {
        _userService = userService;
    }

    // GET: /api/user/create ??
    // This returns all transactions in the database
    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserRequestDto request){
        // This tells EF Core: "Go to the Users table and return them as a list."
        bool success = await _userService.CreateNewUserAsync(request);

        if(!success){
            return BadRequest("Could not create new user");
        }

        // This likely needs to return a real status code
        return Ok(new { Message = "User created and accounts synced!" }); 

    }

    [HttpGet]
    public async Task<IActionResult> GetUser([FromBody] GetUserRequestDto request){
        var user = await _userService.GetUserAsync(request);
        if(user is null){
            return NotFound("User not found");
        }
        return Ok(user);
    }

    // [HttpGet]
    // public async Task<IActionResult> GetUser([FromBody] CreateUserRequestDto request){
    //     // This tells EF Core: "Go to the Users table and return them as a list."
    //     bool success = await _userService.CreateNewUser(request);

    //     if(!success){
    //         return BadRequest("Could not create new user")
    //     }

    //     // This likely needs to return a real status code
    //     return Ok(new { Message = "User created and accounts synced!" }); 

    // }

}