namespace EasyBudget.Api.Services.Interfaces;

using EasyBudget.Api.DTO;

public interface IWebhookService
{
    bool verifyTellerWebhookSignature(string tellerSignature, TellerWebhookDto dto);
    Task<TellerWebhookDto> ConsumeTellerWebhook(TellerWebhookDto dto);
}
