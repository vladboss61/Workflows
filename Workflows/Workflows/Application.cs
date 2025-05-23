using System;
using System.Threading.Tasks;
using WorkflowCore.Interface;
using WorkflowsEx.DogsApi;
using WorkflowsEx.DogsApi.Models;
using WorkflowsEx.GithubApi;
using WorkflowsEx.GithubApi.Models;
using WorkflowsEx.Workflows;
using WorkflowsEx.Workflows.Data;
using Microsoft.Extensions.Logging;

namespace WorkflowsEx;

internal class Application
{
    private readonly IWorkflowHost _workflowHost;
    private readonly IGithubRepository _githubRepository;
    private readonly IDogsRepository _dogsRepository;
    private readonly ILogger<Application> _logger;

    public Application(
        IWorkflowHost workflowHost,
        IGithubRepository githubRepository,
        IDogsRepository dogsRepository,
        ILogger<Application> logger)
    {
        _workflowHost = workflowHost;
        _githubRepository = githubRepository;
        _dogsRepository = dogsRepository;
        _logger = logger;
    }

    public async Task RunAsync()
    {
        _logger.LogInformation("🚀 ~ Start Application ~ 🚀");

        GithubUserResponse githubUserResponse = await _githubRepository.GetUserAsync("vladboss61");

        DogRandomResponse randomDog = await _dogsRepository.GetRandomDogAsync();
        Console.WriteLine(githubUserResponse.ToString());

        GithubRepositoriesResponse githubRepositoriesResponse = await _githubRepository.GetRepositoriesAsync(
            new GithubQueryParamsRequest
            {
                Query = "language:python",
                Sort = nameof(GithubSortParams.Stars),
                Order = nameof(Order.Desc)
            });

        Console.WriteLine(githubRepositoriesResponse.ToString());

        await StartWorkflowsExAsync();
    }
    private async Task StartWorkflowsExAsync()
    {
        _workflowHost.Start();
        
        Console.CancelKeyPress += (object sender, ConsoleCancelEventArgs e) => _workflowHost.Stop();

        await _workflowHost.StartWorkflow(nameof(OrderWorkflow), 1, new OrderData { OrderId = Guid.NewGuid().ToString() });
    }
}
