namespace EasyBudget.Api.Controllers;

using Microsoft.AspNetCore.Mvc;
using EasyBudget.Api.DTO;
using EasyBudget.Api.Services;
using System.Security.Claims;
using EasyBudget.Api.Services.Cache;
using EasyBudget.Api.Services.Interfaces;

[ApiController]
[Route("api/enrollment")]
public class EnrollmentController(
    IEnrollmentService enrollmentService,
    INonceService nonceService,
    IRedisCacheService redisCacheService,
    ILogger<EnrollmentController> logger
) : ControllerBase
{
    /// <summary>
    /// Generates a nonce for the Teller Connect flow.
    /// Call this before opening Teller Connect to get a nonce for signature verification.
    /// </summary>
    [HttpGet("nonce")]
    public async Task<IActionResult> GetNonce()
    {
        var auth0Id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(auth0Id))
        {
            logger.LogWarning("Unauthorized access attempt to GetNonce");
            return Unauthorized();
        }

        var nonce = await nonceService.GenerateNonceAsync(auth0Id);
        return Ok(new NonceResponseDto(nonce));
    }

    [HttpPost]
    public async Task<IActionResult> CreateEnrollment([FromBody] CreateEnrollmentDto dto)
    {
        var auth0Id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(auth0Id))
        {
            logger.LogWarning("Unauthorized access attempt to CreateEnrollment");
            return Unauthorized();
        }

        // Frontend will send enrollment ID and we will idempotently create enrollment
        var cacheKey = redisCacheService.createCacheKey("enrollment_creation", dto.EnrollmentId);
        var existingResponse = await redisCacheService.GetCacheKeyAsync<object>(cacheKey);

        if (existingResponse != null)
        {
            logger.LogInformation("Idempotent enrollment creation detected for EnrollmentId: {EnrollmentId}",
             dto.EnrollmentId);
            return Ok(existingResponse);
        }

        // Verify the nonce and signatures
        var nonce = await nonceService.ConsumeNonceAsync(auth0Id);
        if (nonce is null)
        {
            logger.LogWarning("No nonce found for user {Auth0Id} during enrollment", auth0Id);
            return BadRequest("Invalid or expired session. Please try connecting again.");
        }

        var isValidSignature = nonceService.VerifySignatures(
            nonce,
            dto.AccessToken,
            dto.UserId,
            dto.EnrollmentId,
            dto.Environment,
            dto.Signatures
        );

        if (!isValidSignature)
        {
            logger.LogWarning("Invalid signature for enrollment {EnrollmentId}", dto.EnrollmentId);
            return BadRequest("Invalid enrollment signature. Please try connecting again.");
        }

        bool success = await enrollmentService.CreateEnrollmentAsync(auth0Id, dto);
        if (!success)
        {
            return BadRequest("Could not create new enrollment");
        }

        var successResponse = new
        {
            Message = $"Enrollment created for {dto.InstitutionName}",
            dto.InstitutionName
        };

        int twentyFourHoursInMinutes = 24 * 60;
        await redisCacheService.SetCacheKeyAsync(cacheKey, successResponse,
            TimeSpan.FromHours(twentyFourHoursInMinutes));

        return Ok(successResponse);
    }

    [HttpGet("{enrollmentId}")]
    public async Task<IActionResult> GetEnrollment(string enrollmentId)
    {
        var auth0Id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(auth0Id))
        {
            return Unauthorized("Could not determine user identity");
        }

        var enrollment = await enrollmentService.GetEnrollmentAsync(enrollmentId, auth0Id);

        if (enrollment is null)
        {
            return NotFound("Enrollment not found");
        }

        return Ok(new { Message = $"Enrollment found for Institution {enrollment.InstitutionName}", enrollment.InstitutionName });
    }

    [HttpGet]
    public async Task<IActionResult> GetAllEnrollments()
    {
        var auth0Id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(auth0Id))
        {
            return Unauthorized("Could not determine user identity");
        }

        var enrollments = await enrollmentService.GetAllEnrollmentsAsync(auth0Id);

        if (enrollments is null || !enrollments.Any())
        {
            return NotFound("No enrollments found");
        }

        return Ok(enrollments);
    }



}