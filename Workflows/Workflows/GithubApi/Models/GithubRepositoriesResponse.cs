namespace WorkflowsEx.GithubApi.Models;

using System.Text;
using System.Text.Json.Serialization;

public class GithubRepositoriesResponse
{
    [JsonPropertyName("total_count")]
    public int TotalCount { get; set; }

    public GithubRepositoryResponse[] Items { get; set; }

    public override string ToString()
    {
        StringBuilder stringBuilder = new StringBuilder();

        stringBuilder.AppendLine("Github Repos:");
        stringBuilder.AppendLine($"Total Count: {TotalCount}");
        stringBuilder.AppendLine("===========================");

        foreach (GithubRepositoryResponse item in Items)
        {
            stringBuilder.AppendLine($"[Id]: {item.Id} [Repo Name]: {item.Name}");
        }

        return stringBuilder.ToString();
    }
}
