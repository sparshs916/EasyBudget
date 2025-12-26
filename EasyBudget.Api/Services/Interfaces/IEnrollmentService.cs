namespace EasyBudget.Api.Services.Interfaces;

using EasyBudget.Api.DTO;
using EasyBudget.Api.Models;

public interface IEnrollmentService
{
    Task<bool> CreateEnrollmentAsync(string auth0Id, CreateEnrollmentDto dto);
    Task<Enrollment?> GetEnrollmentAsync(string enrollmentId, string auth0Id);
    Task<IEnumerable<Enrollment>?> GetEnrollmentsAsync(string userId, string auth0Id);
    Task<IEnumerable<Enrollment>?> GetAllEnrollmentsAsync(string auth0Id);
}
