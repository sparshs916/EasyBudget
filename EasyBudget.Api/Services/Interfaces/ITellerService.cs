namespace EasyBudget.Api.Services;

using EasyBudget.Api.DTO;

public interface ITellerService
{
    Task<BankAccountDto[]> FetchAllBankAccountsAsync(string AccessToken,
            CancellationToken cancellationToken = default);

    Task<TransactionDto[]> FetchAllTransactionsAsync(string accessToken, string accountId,
            CancellationToken cancellationToken = default); 
}
