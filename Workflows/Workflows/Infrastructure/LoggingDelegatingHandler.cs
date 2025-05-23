namespace WorkflowsEx.Infrastructure;

using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

public sealed class LoggingDelegatingHandler : DelegatingHandler
{
    private readonly ILogger<LoggingDelegatingHandler> _logger;

    public LoggingDelegatingHandler(ILogger<LoggingDelegatingHandler> logger)
    {
        _logger = logger;
    }
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Request {request} is started...");

        HttpResponseMessage responseMessage = await base.SendAsync(request, cancellationToken);

        string responseBody = await responseMessage.Content.ReadAsStringAsync();

        _logger.LogInformation($"Response {responseMessage}{Environment.NewLine}Body: {responseBody}.");

        return responseMessage;
    }
}
