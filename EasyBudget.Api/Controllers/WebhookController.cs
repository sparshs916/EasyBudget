namespace EasyBudget.Api.Controllers;

using Microsoft.AspNetCore.Mvc;
using EasyBudget.Api.DTO;
using EasyBudget.Api.Services;
using System.Security.Claims;
using EasyBudget.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;

[AllowAnonymous]
[ApiController]
[Route("api/webhooks")]
public class WebhookController(
    IWebhookService webhookService,
    ILogger<WebhookController> logger
) : ControllerBase
{
    [HttpPost("teller")]
    public async Task<IActionResult> HandleTellerWebhooksAsync([FromBody] TellerWebhookDto dto)
    {
        if (dto is null)
        {
            logger.LogError("Teller webhook DTO is null");
            return BadRequest();
        }

        return Ok(new { Message = $"webhook ID={dto.WebhookId}\n" });


        // bool success = await webhookService.ConsumeTellerWebhook(dto);
        // if (!success)
        // {
        //     logger.LogWarning("ConsumeTellerWebhook failed");
        // }
        // return Ok();

    }

}