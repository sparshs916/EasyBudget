namespace EasyBudget.Api.Services;

using EasyBudget.Api.DTO;

public interface ITellerService
{
    Task<BankAccountDto[]> GetUserBankAccountsAsync(string token, CancellationToken cancellationToken = default);
}
