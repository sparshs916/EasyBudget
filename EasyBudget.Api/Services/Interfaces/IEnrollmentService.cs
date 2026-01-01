namespace EasyBudget.Api.Services.Interfaces;

using EasyBudget.Api.DTO;
using EasyBudget.Api.Models;

public interface IEnrollmentService
{
    Task<bool> CreateEnrollmentAsync(string auth0Id, CreateEnrollmentDto dto,
    CancellationToken cancellationToken = default);
    Task<Enrollment?> GetEnrollmentAsync(string enrollmentId, string auth0Id);
    Task<IEnumerable<Enrollment>?> GetAllEnrollmentsAsync(string auth0Id);

    /// <summary>
    /// Gets the decrypted access token for the enrollment associated with a bank account.
    /// </summary>
    /// <param name="accountId">The Teller account ID.</param>
    /// <param name="auth0Id">The user's Auth0 ID for authorization.</param>
    /// <returns>The decrypted access token, or null if not found.</returns>
    Task<string?> GetAccessTokenForAccountAsync(string accountId, string auth0Id);
}
