namespace EasyBudget.Api.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EasyBudget.Api.Data;
using EasyBudget.Api.Models;
using EasyBudget.Api.Services;
using EasyBudget.Api.Services.Cache;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using EasyBudget.Api.DTO;
using EasyBudget.Api.Services.Interfaces;

[ApiController]
[Route("api/[controller]")]
public class BankAccountController(
    IBankAccountService bankAccountService,
    IRedisCacheService redisCacheService) :
    ControllerBase
{
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> AddAllBankAccounts()
    {
        var auth0Id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(auth0Id))
        {
            return Unauthorized("Could not determine user identity");
        }

        var accessToken = Request.Headers["access_token"].FirstOrDefault();
        if (accessToken == null)
        {
            return BadRequest("Access token is required");
        }

        string cache_key = 
            redisCacheService.createCacheKey("bank_account_creation", accessToken);

        var cachedAccounts =
            await redisCacheService.GetCacheKeyAsync<BankAccountDto[]>(cache_key);

        if (cachedAccounts != null)
        {
            return Ok(new { Message = "Bank accounts retrieved from cache", 
                Accounts = cachedAccounts });
        }

        // Fetch and create bank accounts
        BankAccountDto[] createdAccountsDto =
            await bankAccountService.CreateBankAccountAsync(accessToken, auth0Id);

        if (createdAccountsDto is null || createdAccountsDto.Length == 0)
        {
            return BadRequest("Could not sync bank accounts");
        }

        // Cache for 24 hours
        int twentyFourHoursInMinutes = 24 * 60;
        await redisCacheService.SetCacheKeyAsync(cache_key,
            new { SyncedAt = DateTime.UtcNow }, 
            TimeSpan.FromMinutes(twentyFourHoursInMinutes));

        return Ok(new { Message = "Bank accounts created successfully!" });
    }

    [Authorize]
    [HttpGet("{enrollmentId}")]
    public async Task<IActionResult> GetBankAccounts(string enrollmentId)
    {
        var auth0Id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(auth0Id))
        {
            return Unauthorized("Could not determine user identity");
        }

        var bankAccounts =
        await bankAccountService.GetBankAccountsAsync(enrollmentId, auth0Id);

        if (bankAccounts is null || bankAccounts.Length == 0)
        {
            return NotFound("No bank accounts found");
        }

        return Ok(bankAccounts);
    }
}