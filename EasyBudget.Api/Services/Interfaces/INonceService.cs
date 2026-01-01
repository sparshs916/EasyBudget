namespace EasyBudget.Api.Services.Interfaces;

/// <summary>
/// Service for generating and validating nonces used in Teller Connect enrollment flow.
/// Nonces prevent replay attacks by ensuring enrollment objects were created during the current session.
/// </summary>
public interface INonceService
{
    /// <summary>
    /// Generates a cryptographically secure nonce and stores it in Redis with the user's session.
    /// </summary>
    /// <param name="auth0Id">The user's Auth0 ID to associate the nonce with.</param>
    /// <returns>The generated nonce string.</returns>
    Task<string> GenerateNonceAsync(string auth0Id);

    /// <summary>
    /// Retrieves and removes the nonce associated with the user (one-time use).
    /// </summary>
    /// <param name="auth0Id">The user's Auth0 ID.</param>
    /// <returns>The nonce if found, null otherwise.</returns>
    Task<string?> ConsumeNonceAsync(string auth0Id);

    /// <summary>
    /// Verifies that the provided signatures are valid for the enrollment data.
    /// </summary>
    /// <param name="nonce">The nonce that was used during TellerConnect setup.</param>
    /// <param name="accessToken">The access token from the enrollment.</param>
    /// <param name="userId">The Teller user ID from the enrollment.</param>
    /// <param name="enrollmentId">The enrollment ID.</param>
    /// <param name="environment">The Teller environment (sandbox, development, production).</param>
    /// <param name="signatures">The signatures array from the enrollment object.</param>
    /// <returns>True if at least one signature is valid.</returns>
    bool VerifySignatures(string nonce, string accessToken, string userId,
        string enrollmentId, string environment, string[] signatures);
}
