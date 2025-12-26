namespace EasyBudget.Api.DTO;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

using EasyBudget.Api.Models;

public record CreateEnrollmentDto(
    string AccessToken,
    string UserId,
    string EnrollmentId,
    string InstitutionId,
    string InstitutionName
    // TODO: add signature?
);

