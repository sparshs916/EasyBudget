namespace EasyBudget.Api.Services.Interfaces;

using EasyBudget.Api.DTO;
using EasyBudget.Api.Models;

public interface IBankAccountService
{
    Task<BankAccountDto[]> GetBankAccountsAsync(string enrollmentId, string auth0Id);
}
