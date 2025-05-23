namespace WorkflowsEx.GithubApi.Models;

using Refit;

public class GithubQueryParamsRequest
{
    [AliasAs("q")]
    public string Query { get; set; }

    public string Sort { get; set; }

    public string Order { get; set; }
}
