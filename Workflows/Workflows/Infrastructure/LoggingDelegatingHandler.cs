namespace WorkflowsEx.Infrastructure;

using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

public sealed class LoggingDelegatingHandler : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {

        Console.WriteLine($"Request {request} is started...");

        HttpResponseMessage responseMessage = await base.SendAsync(request, cancellationToken);

        string responseBody = await responseMessage.Content.ReadAsStringAsync();

        Console.WriteLine($"Response {responseMessage}{Environment.NewLine}Body: {responseBody}.");

        return responseMessage;
    }
}
