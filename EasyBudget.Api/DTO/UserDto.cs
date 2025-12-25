namespace EasyBudget.Api.DTO;
using System.ComponentModel.DataAnnotations;

public record CreateUserRequestDto(
    [param: Required] [param: MaxLength(32)] string Username,
    [param: Required] [param: EmailAddress] string Email,
    [param: Required] [param: MaxLength(50)] string Auth0Id
);

public record GetUserRequestDto(
    [param: Required] [param: MaxLength(50)] string Auth0Id
);