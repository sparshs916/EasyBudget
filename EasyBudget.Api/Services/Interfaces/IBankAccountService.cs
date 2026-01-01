namespace EasyBudget.Api.Services.Interfaces;

using EasyBudget.Api.DTO;


public interface IBankAccountService
{
    Task<BankAccountDto[]>
    CreateBankAccountAsync(string accessToken, string auth0Id,
    CancellationToken cancellationToken = default);
    Task<BankAccountDto[]>
    GetBankAccountsbyIdAsync(string enrollmentId,
        string auth0Id, CancellationToken cancellationToken = default);

    Task<BankAccountDto[]>
    GetBankAccountsAsync(string auth0Id, CancellationToken cancellationToken = default);

}
