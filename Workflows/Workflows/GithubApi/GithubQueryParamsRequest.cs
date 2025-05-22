namespace WorkflowsEx.GithubApi;

using Newtonsoft.Json;
using Refit;

public enum GithubSortParams
{
    Stars
}

public enum Order
{
    Asc,
    Desc,
}

public class GithubQueryParamsRequest
{
    [AliasAs("q")]
    public string Query { get; set; }

    public string Sort { get; set; }

    public string Order { get; set; }
}
