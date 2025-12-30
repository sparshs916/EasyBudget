namespace EasyBudget.Api.Services;

using EasyBudget.Api.DTO;

public interface ITellerService
{
    Task<BankAccountDto[]> FetchBankAccountsAsync(string AccessToken,
            CancellationToken cancellationToken = default);
}
