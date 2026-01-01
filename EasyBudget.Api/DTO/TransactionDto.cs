using System.Text.Json.Serialization;

namespace EasyBudget.Api.DTO;

public record TransactionCounterpartyDto(
    [property: JsonPropertyName("type")] string type,
    [property: JsonPropertyName("name")] string name
);
public record TransactionDetailsDto(
    [property: JsonPropertyName("category")] string Category,
    [property: JsonPropertyName("organization")] decimal Amount,
    [property: JsonPropertyName("counterparty")] TransactionCounterpartyDto Counterparty,
    [property: JsonPropertyName("processing_status")] string ProcessingStatus
);
public record TransactionDto(
    [property: JsonPropertyName("amount")] decimal Amount,
    [property: JsonPropertyName("account_id")] string AccountId,
    [property: JsonPropertyName("id")] string TransactionId,
    [property: JsonPropertyName("date")] DateOnly Date,
    [property: JsonPropertyName("type")] string Type,
    [property: JsonPropertyName("status")] string Status,
    [property: JsonPropertyName("running_balance")] decimal? RunningBalance,
    [property: JsonPropertyName("description")] string Description,
    [property: JsonPropertyName("details")] TransactionDetailsDto Details
);