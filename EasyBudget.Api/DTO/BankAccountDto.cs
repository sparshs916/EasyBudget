namespace EasyBudget.Api.DTO;

public record BankAccountDto(
    string LastFour,
    string Subtype,
    string InstitutionId,
    string InstitutionName,
    string EnrollmentId,
    string Currency,
    string Status,
    string AccountName,
    string AccountId
);
