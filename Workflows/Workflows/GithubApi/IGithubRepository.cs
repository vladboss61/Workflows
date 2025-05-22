namespace WorkflowsEx.GithubApi;

using Refit;
using System.Threading.Tasks;

public interface IGithubRepository
{
    [Get("/users/{username}")]
    public Task<GithubUserResponse> GetUserAsync(string username);

    [Get("/search/repositories")]
    public Task<GithubRepositoriesResponse> GetRepositoriesAsync([Query] GithubQueryParamsRequest paramsRequest);
}
