namespace EasyBudget.Api.Services;

using System.Security.Cryptography;
using System.Text;
using EasyBudget.Api.Services.Cache;
using EasyBudget.Api.Services.Interfaces;
using Microsoft.Extensions.Logging;
using NSec.Cryptography;

public sealed class NonceService(
    IRedisCacheService redisCacheService,
    ILogger<NonceService> logger
) : INonceService
{
    private const string NonceCachePrefix = "teller_nonce";
    private static readonly TimeSpan NonceExpiration = TimeSpan.FromMinutes(15);

    public async Task<string> GenerateNonceAsync(string auth0Id)
    {
        // Generate a cryptographically secure random nonce
        var nonceBytes = RandomNumberGenerator.GetBytes(32);
        var nonce = Convert.ToBase64String(nonceBytes);

        // Store in Redis with the user's auth0Id
        var cacheKey = redisCacheService.createCacheKey(NonceCachePrefix, auth0Id);
        await redisCacheService.SetCacheKeyAsync(cacheKey, nonce, NonceExpiration);

        logger.LogDebug("Generated nonce for user {Auth0Id}", auth0Id);
        return nonce;
    }

    public async Task<string?> ConsumeNonceAsync(string auth0Id)
    {
        var cacheKey = redisCacheService.createCacheKey(NonceCachePrefix, auth0Id);
        var nonce = await redisCacheService.GetCacheKeyAsync<string>(cacheKey);

        if (nonce is null)
        {
            logger.LogWarning("No nonce found for user {Auth0Id}", auth0Id);
            return null;
        }

        // Remove the nonce after retrieving (one-time use)
        await redisCacheService.RemoveCacheKeyAsync(cacheKey);
        logger.LogDebug("Consumed nonce for user {Auth0Id}", auth0Id);

        return nonce;
    }

    public bool VerifySignatures(string nonce, string accessToken, string userId,
        string enrollmentId, string environment, string[] signatures)
    {
        if (signatures is null || signatures.Length == 0)
        {
            logger.LogWarning("No signatures provided for verification");
            return false;
        }

        // Get the Teller public key from environment
        // TODO: Remove when testing is done!
        var tellerPublicKeyBase64 = Environment.GetEnvironmentVariable("TELLER_SIGNING_PUBLIC_KEY");
        if (string.IsNullOrEmpty(tellerPublicKeyBase64))
        {
            logger.LogWarning("TELLER_SIGNING_PUBLIC_KEY not configured, skipping signature verification");
            // In development/sandbox, skip verification
            var tellerEnv = Environment.GetEnvironmentVariable("TELLER_ENVIRONMENT") ?? "sandbox";
            if (tellerEnv == "sandbox")
            {
                logger.LogInformation("Sandbox environment - skipping signature verification");
                return true;
            }
            return false;
        }

        // Build the data that was signed: nonce.accessToken.userId.enrollmentId.environment
        var dataToVerify = $"{nonce}.{accessToken}.{userId}.{enrollmentId}.{environment}";
        var dataBytes = Encoding.UTF8.GetBytes(dataToVerify);

        try
        {
            var publicKeyBytes = Convert.FromBase64String(tellerPublicKeyBase64);
            var algorithm = SignatureAlgorithm.Ed25519;
            var publicKey = PublicKey.Import(algorithm, publicKeyBytes, KeyBlobFormat.RawPublicKey);

            // Try to verify at least one signature
            foreach (var signatureBase64 in signatures)
            {
                try
                {
                    var signatureBytes = Convert.FromBase64String(signatureBase64);
                    if (algorithm.Verify(publicKey, dataBytes, signatureBytes))
                    {
                        logger.LogInformation("Successfully verified enrollment signature");
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    logger.LogDebug(ex, "Failed to verify signature: {Signature}", signatureBase64);
                }
            }

            logger.LogWarning("No valid signatures found for enrollment");
            return false;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error during signature verification");
            return false;
        }
    }
}
