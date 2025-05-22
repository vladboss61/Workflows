
namespace WorkflowsEx.GithubApi;

using Refit;
using System.Threading.Tasks;

// Alternatives for set-up User-Agent header compare to in Program.cs
[Headers("User-Agent: my-refit-app")]
public interface IGithubRepository
{
    [Get("/users/{username}")]
    public Task<GithubUserResponse> GetUserAsync(string username);

    [Get("/search/repositories")]
    public Task<GithubRepositoriesResponse> GetRepositoriesAsync([Query] GithubQueryParamsRequest paramsRequest);
}
