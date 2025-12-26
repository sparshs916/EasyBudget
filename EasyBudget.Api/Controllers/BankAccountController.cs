namespace EasyBudget.Api.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EasyBudget.Api.Data;
using EasyBudget.Api.Models;
using EasyBudget.Api.Services;

[ApiController]
[Route("api/[controller]")] 
public class BankAccountController(
    BankAccountService bankAccountService,
    TellerService tellerService) :
    ControllerBase
{
   

    // // GET: /api/bankaccount
    // // This returns all transactions in the database
    // [HttpGet]
    // public async Task<bool> FetchAllBankAccountsAsync()
    // {

    //     tellerService.GetUserBankAccountsAsync("some-access-token"); 
        
    // }

    // [HttpPost]
    // public async Task<ActionResult<BankAccount>> PostBankAccount(BankAccount bankaccount)
    // {
        
    // }
}