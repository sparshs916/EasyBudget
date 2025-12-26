namespace EasyBudget.Api.Controllers;

using Microsoft.AspNetCore.Mvc;
using EasyBudget.Api.DTO;
using EasyBudget.Api.Services;
using System.Security.Claims;

[ApiController]
[Route("api/enrollment")]
public class EnrollmentController(
    EnrollmentService enrollmentService
) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateEnrollment([FromBody] CreateEnrollmentDto dto)
    {
        var auth0Id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(auth0Id))
        {
            return Unauthorized("Could not determine user identity");
        }

        bool success = await enrollmentService.CreateEnrollmentAsync(auth0Id, dto);

        if (!success)
        {
            return BadRequest("Could not create new enrollment");
        }

        return Ok(new { Message = $"Enrollment created for {dto.InstitutionName}", dto.InstitutionName });
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