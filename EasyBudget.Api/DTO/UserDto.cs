namespace EasyBudget.Api.DTO;

using System.ComponentModel.DataAnnotations;

public record CreateUserRequestDto(
    [param: Required][param: MaxLength(32)] string Username,
    [param: Required][param: EmailAddress] string Email);

/// <summary>
/// DTO for Auth0 Post-Registration webhook
/// </summary>
public record Auth0WebhookUserDto(
    [param: Required] string Auth0Id,
    [param: Required][param: EmailAddress] string Email,
    [param: Required][param: MaxLength(32)] string Username);
