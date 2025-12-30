namespace EasyBudget.Api.DTO;
using System.Text.Json.Serialization;
public record InstitutionDto(
    [property: JsonPropertyName("id")] string Id,
    [property: JsonPropertyName("name")] string Name
);
public record BankAccountDto(
    [property: JsonPropertyName("id")] string AccountId,
    [property: JsonPropertyName("name")] string AccountName,
    [property: JsonPropertyName("currency")] string Currency,
    [property: JsonPropertyName("enrollment_id")] string EnrollmentId,
    [property: JsonPropertyName("institution")] InstitutionDto Institution,
    [property: JsonPropertyName("last_four")] string LastFour,
    [property: JsonPropertyName("subtype")] string Subtype,
    [property: JsonPropertyName("type")] string Type,
    [property: JsonPropertyName("status")] string Status
);
