namespace EasyBudget.Api.Services;

using EasyBudget.Api.Data;
using EasyBudget.Api.DTO;
using EasyBudget.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using EasyBudget.Api.Models;

public sealed class TransactionService(
    ApiDbContext context,
    ILogger<TransactionService> logger,
    ITellerService tellerService
) : ITransactionService
{

    /// <summary>
    /// Syncs transactions from Teller API for all bank accounts associated with the user.
    /// Access token is retrieved from each enrollment in the database.
    /// </summary>
    /// <param name="auth0Id">The user's Auth0 ID.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Array of <see cref="TransactionDto"/></returns>
    public async Task<TransactionDto[]> CreateTransactionsAsync(string auth0Id,
        CancellationToken cancellationToken = default)
    {
        try
        {
            // Fetch bank accounts for this user (through enrollment relationship)
            var bankAccounts = await context.BankAccounts
                .Include(ba => ba.Enrollment)
                    .ThenInclude(e => e!.User)
                .Where(ba => ba.Enrollment != null &&
                             ba.Enrollment.User != null &&
                             ba.Enrollment.User.Auth0Id == auth0Id)
                .ToListAsync(cancellationToken);

            if (bankAccounts is null || bankAccounts.Count == 0)
            {
                logger.LogError("No bank accounts found for User {Auth0Id}", auth0Id);
                return Array.Empty<TransactionDto>();
            }

            var allTransactions = new List<TransactionDto>();
            foreach (var account in bankAccounts)
            {
                // Get access token from the enrollment
                var accessToken = account.Enrollment?.AccessToken;
                if (string.IsNullOrEmpty(accessToken))
                {
                    logger.LogWarning("No access token found for account {AccountId}", account.AccountId);
                    continue;
                }

                TransactionDto[]? accountTransactions =
                    await tellerService.FetchAllTransactionsAsync(
                        accessToken, account.AccountId, cancellationToken);

                if (accountTransactions is null || accountTransactions.Length == 0)
                {
                    logger.LogWarning("No transactions found to create for AccountId {AccountId}.", account.AccountId);
                    continue;
                }

                foreach (var transactionDto in accountTransactions)
                {
                    var transaction = new Transaction
                    {
                        AccountGuid = account.Guid,
                        AccountId = account.AccountId,
                        TransactionId = transactionDto.TransactionId,
                        Amount = transactionDto.Amount,
                        Date = transactionDto.Date,
                        Description = transactionDto.Description,
                        Type = transactionDto.Type,
                        Status = transactionDto.Status,
                        RunningBalance = transactionDto.RunningBalance,
                        Category = transactionDto.Details?.Category,
                        ProcessingStatus = transactionDto.Details?.ProcessingStatus,
                        CounterpartyName = transactionDto.Details?.Counterparty?.name,
                        CounterpartyType = transactionDto.Details?.Counterparty?.type
                    };
                    context.Transactions.Add(transaction);
                    allTransactions.Add(transactionDto);
                }
            }

            await context.SaveChangesAsync(cancellationToken);
            return allTransactions.ToArray();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while fetching transactions from Teller API.");
            return Array.Empty<TransactionDto>();
        }
    }

    public async Task<TransactionDto[]> GetTransactionsAsync(string accountId,
        DateOnly? startDate, DateOnly? endDate,
        CancellationToken cancellationToken = default)
    {
        var transactions = await context.Transactions
            .Where(t => t.AccountId == accountId
                     && (!startDate.HasValue || t.Date >= startDate.Value)
                     && (!endDate.HasValue || t.Date <= endDate.Value))
            .OrderBy(t => t.Date)
            .Select(t => new TransactionDto(
                t.Amount,
                t.TransactionId,
                t.AccountId,
                t.Date,
                t.Description,
                t.Type,
                t.RunningBalance,
                t.Status,
                new TransactionDetailsDto(
                    t.Category ?? "none",
                    t.RunningBalance ?? 0,
                    new TransactionCounterpartyDto(t.CounterpartyType ?? string.Empty, t.CounterpartyName ?? string.Empty),
                    t.ProcessingStatus ?? "none"
                )
            ))
            .ToArrayAsync(cancellationToken);

        return transactions;
    }
}