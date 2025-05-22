using Newtonsoft.Json;

namespace WorkflowsEx.GithubApi;

public class GithubRepositoryResponse
{
    public int Id { get; set; }

    public string Name { get; set; }
}

public class GithubRepositoriesResponse
{
    [JsonProperty("total_count")]
    public int TotalCount { get; set; }

    public GithubRepositoryResponse[] Items { get; set; }
}
