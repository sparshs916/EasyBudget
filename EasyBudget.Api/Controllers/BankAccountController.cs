namespace EasyBudget.Api.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EasyBudget.Api.Data;
using EasyBudget.Api.Models;

[ApiController]
[Route("api/bankaccount")] // This makes the URL: /api/bankaccount
public class BankAccountController : ControllerBase
{
    private readonly ApiDbContext _context;

    // Dependency Injection: .NET gives us the DB connection automatically
    public BankAccountController(ApiDbContext context)
    {
        _context = context;
    }

    // GET: /api/bankaccount
    // This returns all transactions in the database
    [HttpGet]
    public async Task<ActionResult<IEnumerable<BankAccount>>> GetBankAccounts()
    {
        // This tells EF Core: "Go to the BankAccounts table, 
        // JOIN the Transactions table so I can see the spending, 
        // and turn the whole thing into a List."
        return await _context.BankAccounts
            .Include(b => b.Transactions) 
            .ToListAsync();
    }

    [HttpPost]
    public async Task<ActionResult<BankAccount>> PostBankAccount(BankAccount bankaccount)
    {
        _context.BankAccounts.Add(bankaccount); // Use the plural name from your DbContext
        await _context.SaveChangesAsync();

        // This tells the user: "Success! I created it at this ID."
        return CreatedAtAction(nameof(GetBankAccounts), new { id = bankaccount.AccountId }, bankaccount);
    }
}