namespace WorkflowsEx.GithubApi;

using Newtonsoft.Json;

public sealed class GithubUserResponse
{
    public string Login { get; set; }

    public int Id { get; set; }

    [JsonProperty("avatar_url")]
    public string AvatarUrl { get; set; }

    public override string ToString()
    {
        return $"[Id]: {Id} [Login]: {Login} [AvatarUrl]: {AvatarUrl}";
    }
}
