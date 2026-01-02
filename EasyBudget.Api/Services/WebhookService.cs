namespace EasyBudget.Api.Services;

using System.Text.Json;
using EasyBudget.Api.Data;
using EasyBudget.Api.DTO;
using EasyBudget.Api.Services.Interfaces;
using System.Security.Cryptography;
public sealed class WebhookService(
    ApiDbContext context,
    ILogger<WebhookService> logger,
    IHttpClientFactory httpClientFactory
) : IWebhookService
{
    private readonly int timestampSubstrIndex = 2;
    private readonly int signatureSubstrIndex = 3;
    private readonly int timestampStr = 0;
    public bool verifyTellerWebhookSignature(string tellerSignature,
        TellerWebhookDto dto)
    {
        var tellerSignedKeyBase64 =
        Environment.GetEnvironmentVariable("TELLER_SIGNING_SECRET");

        if (string.IsNullOrEmpty(tellerSignedKeyBase64))
        {
            logger.LogWarning("TELLER_SIGNING_SECRET not configured");
            return false;
        }
        else if (string.IsNullOrEmpty(tellerSignature))
        {
            logger.LogError("Teller signature is NULL");
            return false;
        }

        // timestamp format will be t=123125 so start after '='
        string timestamp = tellerSignature.Split(",")[timestampStr].Substring(
            timestampSubstrIndex);
        logger.LogInformation($"timestamp {timestamp}");

        int i = 0;
        string[] signatures = Array.Empty<string>();

        foreach (string signature in tellerSignature.Split(","))
        {
            i++;
            if (signature.Contains($"v{i}="))
            {
                //signature format will be v{i}=123125 so start after '='
                signatures.Append(signature.Substring(signatureSubstrIndex));
                logger.LogInformation($"signature {i}:{signature}");
            }
        }


        if (signatures is null || signatures.Length == 0)
        {
            logger.LogError("Extracted signatures are null or empty");
            return false;
        }

        // Check if timestamp is not older than 3 minutes (replay attack prevention)
        if (!long.TryParse(timestamp, out long unixTimestamp))
        {
            logger.LogError("Invalid timestamp format");
            return false;
        }

        var currentUnixTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        var timeDifferenceSeconds = currentUnixTime - unixTimestamp;
        if (timeDifferenceSeconds > 180) // 3 minutes = 180 seconds
        {
            logger.LogWarning("Webhook timestamp is older than 3 minutes. Rejecting as potential replay attack.");
            return false;
        }

        // Create signed message: timestamp.json_body
        var signedMessage = $"{timestamp}.{JsonSerializer.Serialize(dto)}";

        // Compute HMAC-SHA256 with signing secret as key
        var signingSecretBytes = Convert.FromBase64String(tellerSignedKeyBase64);
        var signedMessageBytes = System.Text.Encoding.UTF8.GetBytes(signedMessage);

        using (var hmac = new HMACSHA256(signingSecretBytes))
        {
            var computedHash = hmac.ComputeHash(signedMessageBytes);
            var computedSignature = Convert.ToBase64String(computedHash);

            // Check if computed signature matches any of the provided signatures
            foreach (var signature in signatures)
            {
                if (computedSignature == signature.Trim())
                {
                    logger.LogInformation("Webhook signature verified successfully");
                    return true;
                }
            }
        }

        logger.LogWarning("Webhook signature verification failed - no matching signatures found");
        return false;
    }


    public async Task<TellerWebhookDto?> 
    ConsumeTellerWebhook(TellerWebhookDto dto)
    {

        return dto;

    }

}

