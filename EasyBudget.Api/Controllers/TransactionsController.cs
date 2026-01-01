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
[Route("api/transactions")]
public class Transactions(
    ITransactionService transactionService,
    IRedisCacheService redisCacheService,
    ILogger<Transactions> logger
) : ControllerBase
{
    /// <summary>
    /// Syncs transactions from Teller API for all bank accounts.
    /// Access token is retrieved from the database based on the authenticated user.
    /// </summary>
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateTransactionsAsync(CancellationToken cancellationToken = default)
    {
        var auth0Id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(auth0Id))
        {
            logger.LogWarning("Unauthorized access attempt to CreateTransactionsAsync");
            return Unauthorized();
        }

        try
        {
            string cacheKey =
                redisCacheService.createCacheKey("transactions_synced", auth0Id);

            var syncedCache =
                await redisCacheService.GetCacheKeyAsync<object>(cacheKey);

            if (syncedCache is not null)
            {
                return Ok(new
                {
                    Message = "Transactions already synced today",
                });
            }

            // Access token is now retrieved internally by the service
            await transactionService.CreateTransactionsAsync(auth0Id, cancellationToken);

            int twentyFourHoursInMinutes = 24 * 60;
            await redisCacheService.SetCacheKeyAsync(cacheKey,
                new { SyncedAt = DateTime.UtcNow },
            TimeSpan.FromMinutes(twentyFourHoursInMinutes));
            logger.LogInformation("Successfully synced transactions for user {Auth0Id}", auth0Id);

            return Ok(new
            {
                Message = "Transactions created for all bank accounts"
            });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error syncing transactions for user {Auth0Id}", auth0Id);
            return BadRequest();
        }
    }

    /// <summary>
    /// Gets transactions for a specific bank account from the local database.
    /// No access token needed - data is already stored locally.
    /// </summary>
    [Authorize]
    [HttpGet("{accountId}")]
    public async Task<IActionResult> GetTransactionsForAccountAsync(string accountId,
        [FromQuery] DateOnly? startDate, [FromQuery] DateOnly? endDate)
    {
        if (string.IsNullOrEmpty(accountId))
        {
            return BadRequest("AccountId is required");
        }

        var auth0Id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(auth0Id))
        {
            logger.LogWarning("Unauthorized access attempt to GetTransactionsForAccountAsync");
            return Unauthorized();
        }

        // Query from local database - no accessToken needed
        var transactions = await transactionService.GetTransactionsAsync(
            accountId, startDate, endDate);

        return Ok(new
        {
            Message = "Transactions retrieved successfully",
            Transactions = transactions
        });
    }
}