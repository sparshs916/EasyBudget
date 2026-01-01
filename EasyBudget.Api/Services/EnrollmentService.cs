using EasyBudget.Api.Services.Interfaces;

namespace EasyBudget.Api.Services;

using EasyBudget.Api.Data;
using EasyBudget.Api.DTO;
using EasyBudget.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

public sealed class EnrollmentService(
    ApiDbContext context,
    ILogger<EnrollmentService> logger) : IEnrollmentService
{
    public async Task<bool> CreateEnrollmentAsync(string auth0Id, CreateEnrollmentDto dto,
    CancellationToken cancellationToken = default)
    {
        try
        {
            if (dto == null)
            {
                logger.LogError("CreateEnrollmentAsync: dto is null");
                return false;
            }

            User? user = await context.Users
                .FirstOrDefaultAsync(u => u.Auth0Id == auth0Id);

            if (user is null)
            {
                logger.LogError("No User found with Auth0Id");
                return false;
            }

            var existingEnrollments = await context.Enrollments
                .Where(e => e.EnrollmentId == dto.EnrollmentId)
                .ToListAsync(cancellationToken);

            if (existingEnrollments.Count > 0)
            {
                logger.LogInformation("Enrollment with EnrollmentId {EnrollmentId} already exists. Skipping creation.", dto.EnrollmentId);
                return true;
            }

            Enrollment enrollment = new Enrollment
            {
                UserGuid = user.Guid,
                AccessToken = dto.AccessToken,
                EnrollmentId = dto.EnrollmentId,
                InstitutionId = dto.InstitutionId,
                InstitutionName = dto.InstitutionName
            };

            context.Enrollments.Add(enrollment);
            await context.SaveChangesAsync();

            logger.LogInformation(
                "Created new enrollment for user {UserGuid} and Institution {InstitutionName}",
                user.Guid, dto.InstitutionName);

            return true;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while creating enrollment {exception}", ex.Message);
            return false;
        }
    }

    public async Task<Enrollment?> GetEnrollmentAsync(string enrollmentId, string auth0Id)
    {
        try
        {
            Enrollment? enrollment = await context.Enrollments
                .Include(e => e.User)
                .FirstOrDefaultAsync(e => e.EnrollmentId == enrollmentId &&
                            e.User != null &&
                            e.User.Auth0Id == auth0Id);

            if (enrollment is null)
            {
                logger.LogError("No enrollment found for enrollment {EnrollmentId}", enrollmentId);
                return null;
            }

            logger.LogInformation("GET Enrollment: Found enrollment {EnrollmentId}", enrollmentId);
            return enrollment;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while getting enrollment {exception}", ex.Message);
            return null;
        }
    }

    public async Task<IEnumerable<Enrollment>?> GetAllEnrollmentsAsync(string auth0Id)
    {
        try
        {
            var enrollments = await context.Enrollments
                .Include(e => e.User)
                .Where(e => e.User != null && e.User.Auth0Id == auth0Id)
                .ToListAsync();

            if (enrollments is null || enrollments.Count == 0)
            {
                logger.LogInformation("No enrollments found for user");
                return null;
            }

            logger.LogInformation("GET All Enrollments: Found {Count} enrollments", enrollments.Count);
            return enrollments;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while getting all enrollments {exception}", ex.Message);
            return null;
        }
    }

    public async Task<IEnumerable<Enrollment>?> GetEnrollmentsAsync(string userId, string auth0Id)
    {
        try
        {
            var enrollments = await context.Enrollments
                .Include(e => e.User)
                .Where(e => e.UserId == userId &&
                            e.User != null &&
                            e.User.Auth0Id == auth0Id)
                .ToListAsync();

            if (enrollments is null || enrollments.Count == 0)
            {
                logger.LogError("No enrollments found for user {UserId}", userId);
                return null;
            }

            logger.LogInformation("GET Enrollments: Found {Count} enrollments for user {UserId}", enrollments.Count, userId);
            return enrollments;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while getting enrollments {exception}", ex.Message);
            return null;
        }
    }

    public async Task<string?> GetAccessTokenForAccountAsync(string accountId, string auth0Id)
    {
        try
        {
            // Find the bank account and its associated enrollment
            var bankAccount = await context.BankAccounts
                .Include(ba => ba.Enrollment)
                    .ThenInclude(e => e!.User)
                .FirstOrDefaultAsync(ba => ba.AccountId == accountId &&
                                          ba.Enrollment != null &&
                                          ba.Enrollment.User != null &&
                                          ba.Enrollment.User.Auth0Id == auth0Id);

            if (bankAccount?.Enrollment is null)
            {
                logger.LogWarning("No enrollment found for account {AccountId}", accountId);
                return null;
            }

            logger.LogDebug("Found access token for account {AccountId}", accountId);
            return bankAccount.Enrollment.AccessToken;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting access token for account {AccountId}", accountId);
            return null;
        }
    }
}