namespace EasyBudget.Api.Services.Interfaces;

using EasyBudget.Api.DTO;

public interface ITransactionService
{
    Task<bool> CreateTransactionsAsync(string auth0Id, TransactionDto dto, 
    CancellationToken cancellationToken = default);
}