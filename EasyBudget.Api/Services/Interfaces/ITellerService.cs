namespace EasyBudget.Api.Services.Interfaces;

using EasyBudget.Api.DTO;

public interface ITellerService
{
    Task<BankAccountDto[]> GetEnrollment(string AccessToken, CancellationToken cancellationToken = default);
}
