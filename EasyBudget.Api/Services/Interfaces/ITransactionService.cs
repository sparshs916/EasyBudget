namespace EasyBudget.Api.Services.Interfaces;

using EasyBudget.Api.DTO;

public interface ITransactionService
{
    /// <summary>
    /// Syncs transactions from Teller API for all bank accounts associated with the user.
    /// Access token is retrieved from the database.
    /// </summary>
    Task<TransactionDto[]> CreateTransactionsAsync(string auth0Id,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets transactions from the local database for a specific account.
    /// </summary>
    Task<TransactionDto[]> GetTransactionsAsync(string accountId,
        DateOnly? startDate, DateOnly? endDate,
        CancellationToken cancellationToken = default);
}