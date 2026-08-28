using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace FunctionAppSyncDesk;

public class Function1
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<Function1> _logger;

    public Function1(ILogger<Function1> logger, IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
        _httpClient = httpClientFactory.CreateClient();
    }

    [Function("Get-Departments")]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "departments")] HttpRequest req)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");

        try
        {
            // Ajuste para a URL/Rota exata do SyncDesk que rende a tela
            string targetUrl = "http://localhost:5000/Admin/Departamentos";

            var response = await _httpClient.GetAsync(targetUrl).ConfigureAwait(false);
            var htmlContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            // Retorna o HTML diretamente para renderização no navegador
            return new ContentResult
            {
                Content = htmlContent,
                ContentType = "text/html; charset=utf-8",
                StatusCode = (int)response.StatusCode
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar a interface HTML do SyncDesk.");
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }
    }
}



