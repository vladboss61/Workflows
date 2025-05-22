namespace WorkflowsEx;

using Refit;
using System;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using WorkflowCore.Interface;
using WorkflowsEx.Data;
using WorkflowsEx.GithubApi;
using WorkflowsEx.Workflows;
using Microsoft.Extensions.DependencyInjection;

public static class WorkflowHostFactory
{
    public static IWorkflowHost Create()
    {
        IServiceCollection services = new ServiceCollection();
        services.AddLogging();
        services.AddWorkflow(); // in-memory persistence

        var serviceProvider = services.BuildServiceProvider();
        return serviceProvider.GetService<IWorkflowHost>();
    }
}

public sealed class Program
{
    public static async Task Main(string[] args)
    {
        ConfigurationSettings settings = JsonSerializer.Deserialize<ConfigurationSettings>(File.ReadAllText("appsettings.json"));

        await SampleGithubRepoWithRefitAsync(settings);
        await SampleWorkflowAsync();
    }

    public static async Task SampleGithubRepoWithRefitAsync(ConfigurationSettings settings)
    {
        var httpClient = new HttpClient() { BaseAddress = new Uri(settings.GithubUrl) };

        // Required by many HTTP servers.
        httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("my-refit-app");

        IGithubRepository client = RestService.For<IGithubRepository>(httpClient);

        GithubUserResponse githubUserResponse = await client.GetUserAsync("vladboss61");

        Console.WriteLine(githubUserResponse.ToString());

        GithubRepositoriesResponse githubRepositoriesResponse = await client.GetRepositoriesAsync(
            new GithubQueryParamsRequest
            {
                Query = "language:python",
                Sort = nameof(GithubSortParams.Stars),
                Order = nameof(Order.Desc)
            });

        Console.WriteLine(githubRepositoriesResponse.ToString());
    }

    public static async Task SampleWorkflowAsync()
    {
        var host = WorkflowHostFactory.Create();

        host.RegisterWorkflow<OrderWorkflow, OrderData>();

        host.Start();

        await host.StartWorkflow(nameof(OrderWorkflow), 1, new OrderData { OrderId = Guid.NewGuid().ToString() });

        Console.ReadLine();
        host.Stop();
    }
}
