namespace WorkflowsEx.GithubApi.Models;

using System.Text.Json.Serialization;

public sealed class GithubUserResponse
{
    public string Login { get; set; }

    public int Id { get; set; }

    [JsonPropertyName("avatar_url")]
    public string AvatarUrl { get; set; }

    public override string ToString()
    {
        return $"[Id]: {Id} [Login]: {Login} [AvatarUrl]: {AvatarUrl}";
    }
}
