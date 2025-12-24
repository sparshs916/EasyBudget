namespace EasyBudget.Api.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EasyBudget.Api.Data;
using EasyBudget.Api.Models;

[ApiController]
[Route("api/user")] // This makes the URL: /api/User
public class UserController : ControllerBase
{
    private readonly ApiDbContext _context;

    // Dependency Injection: .NET gives us the DB connection automatically
    public UserController(ApiDbContext context)
    {
        _context = context;
    }

    // GET: /api/user
    // This returns all transactions in the database
    [HttpGet]
    public async Task<ActionResult<IEnumerable<User>>> GetUsers()
    {
        // This tells EF Core: "Go to the Users table and return them as a list."
        return await _context.Users
            .Include(b => b.Enrollments) // Include related Enrollments
            .ToListAsync();
    }

    [HttpPost]
    public async Task<ActionResult<User>> PostUser(User user)
    {
        _context.Users.Add(user); // Use the plural name from your DbContext
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetUsers), new { id = user.Guid }, user);
    }
}