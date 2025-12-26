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
    public async Task<bool> CreateEnrollmentAsync(string auth0Id, CreateEnrollmentDto dto)
    {
        try
        {
            User? user = await context.Users
                .FirstOrDefaultAsync(u => u.Auth0Id == auth0Id);

            if (user is null)
            {
                logger.LogError("No User found with Auth0Id");
                return false;
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
                .FirstOrDefaultAsync(e =>
                    e.EnrollmentId == enrollmentId && e.User != null && e.User.Auth0Id == auth0Id);

            if (enrollment is null)
            {
                logger.LogError("No Enrollment found with EnrollmentId {EnrollmentId}", enrollmentId);
                return null;
            }

            logger.LogInformation("GET Enrollment: Found enrollment for user {Username}", enrollment.User?.Username);
            return enrollment;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred on GET Enrollment {exception}", ex.Message);
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
}