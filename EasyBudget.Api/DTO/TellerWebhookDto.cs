using System.Text.Json.Serialization;
using EasyBudget.Api.Models;

namespace EasyBudget.Api.DTO;

public record TellerWebhookPayloadDto(
    [property: JsonPropertyName("enrollment_id")] string EnrollmentId,
    // Available when "type": "enrollment.disconnected" only
    [property: JsonPropertyName("reason")] string? Reason,
    // Available when "type": "transactions.processed" only
    [property: JsonPropertyName("transactions")] TransactionDto[] ? ProcessesedTransactions,
    // Available when "type": "account.number_verification.processed" only
    [property: JsonPropertyName("account_id")] string ? AccountId,
    [property: JsonPropertyName("status")] string ? Status
);
public record TellerWebhookDto(
    [property: JsonPropertyName("id")] string WebhookId,
    [property: JsonPropertyName("payload")] TellerWebhookPayloadDto Payload,
    [property: JsonPropertyName("timestamp")] string Timestamp,
    [property: JsonPropertyName("type")] string Type
);
