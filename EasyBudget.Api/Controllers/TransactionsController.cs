using StackExchange.Redis;

namespace EasyBudget.Api.Services.Interfaces;


public class Transactions(
    ITransactionService transactionService,
    ILogger<Transactions> logger
)
{
    
}