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

    public async Task<BankAccountDto[]> FetchAllBankAccountsAsync(string AccessToken,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var client = httpClientFactory.CreateClient("Teller");
            // Teller uses Basic Auth: access_token as username, empty password
            var credentials = Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes($"{AccessToken}:"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);

            var bankAccounts = await client.GetFromJsonAsync<BankAccountDto[]>(
                "accounts",
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

    public async Task<TransactionDto[]> FetchAllTransactionsAsync(string accessToken, string accountId,
            CancellationToken cancellationToken = default)
    {
        try
        {
            var client = httpClientFactory.CreateClient("Teller");

            var credentials = Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes($"{accessToken}:"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);

            var transactions = await client.GetFromJsonAsync<TransactionDto[]>(
                $"accounts/{accountId}/transactions",
                cancellationToken: cancellationToken
            );

            logger.LogInformation("transaction data: {Transactions}",
                JsonSerializer.Serialize(transactions));

            logger.LogInformation("Fetched {Count} transactions from Teller API.",
             transactions?.Length ?? 0);
            return transactions ?? Array.Empty<TransactionDto>();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error fetching transactions from Teller API.");
            return Array.Empty<TransactionDto>();
        }
    }

    

}