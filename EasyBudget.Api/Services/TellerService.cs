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
    private readonly string accountsURI = "accounts";
    public async Task<BankAccountDto[]> FetchBankAccountsAsync(string AccessToken,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var client = httpClientFactory.CreateClient("Teller");
            // Teller uses Basic Auth: access_token as username, empty password
            var credentials = Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes($"{AccessToken}:"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);

            var bankAccounts = await client.GetFromJsonAsync<BankAccountDto[]>(
                accountsURI,
                cancellationToken: cancellationToken
            );

            logger.LogInformation("bank account data: {BankAccounts}", 
                JsonSerializer.Serialize(bankAccounts));

            logger.LogInformation("Fetched {Count} bank accounts from Teller API.",
             bankAccounts?.Length ?? 0);
            return bankAccounts ?? Array.Empty<BankAccountDto>();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error fetching bank accounts from Teller API.");
            return Array.Empty<BankAccountDto>();
        }
    }

}