using Refit;
using System.Threading.Tasks;

namespace WorkflowsEx.GithubApi;

public interface IGithubRepository
{
    [Get("/users/{username}")]
    public Task<GithubUserResponse> GetUserAsync(string username);
}
