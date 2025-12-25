namespace EasyBudget.Api.Services;

using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using EasyBudget.Api.DTO;
using Microsoft.Extensions.Logging;

public sealed class TellerService(
    IHttpClientFactory httpClientFactory,
    ILogger<TellerService> logger)
    : ITellerService
{


}