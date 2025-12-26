namespace EasyBudget.Api.Services;

using EasyBudget.Api.Services.Interfaces;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using EasyBudget.Api.DTO;
using Microsoft.Extensions.Logging;

public sealed class TellerService(
    IHttpClientFactory httpClientFactory,
    ILogger<TellerService> logger)
    : ITellerService
{
    public async Task<BankAccountDto[]> GetUserBankAccountsAsync(string AccessToken, 
        CancellationToken cancellationToken = default)
    {
        try
        {
            var client = httpClientFactory.CreateClient("Teller");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", 
                AccessToken);

            var bankAccounts = await client.GetFromJsonAsync<BankAccountDto[]>(
                "bank-accounts",
                cancellationToken: cancellationToken
            );
            
            logger.LogInformation("Fetched {Count} bank accounts from Teller API.", bankAccounts?.Length ?? 0);
            return bankAccounts ?? Array.Empty<BankAccountDto>();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error fetching bank accounts from Teller API.");
            return Array.Empty<BankAccountDto>();
        }
    }

}