namespace EasyBudget.Api.Services;

using EasyBudget.Api.Data;
using EasyBudget.Api.DTO;
using EasyBudget.Api.Services.Interfaces;

public sealed class TransactionService : ITransactionService
{
    private readonly ApiDbContext context;
    private readonly ILogger<TransactionService> logger;

    public TransactionService(
        ApiDbContext context,
        ILogger<TransactionService> logger)
    {
        this.context = context;
        this.logger = logger;
    }

    public Task<bool> CreateTransactionsAsync(string auth0Id, TransactionDto dto,
        CancellationToken cancellationToken = default)
    {
        // TODO: Implement transaction creation logic
        throw new NotImplementedException();
    }
}