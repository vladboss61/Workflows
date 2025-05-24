namespace WorkflowsEx.Infrastructure;

public sealed class AppSettings
{
    public int SpecificData { get; set; }
}

public sealed class ConfigurationSettings
{
    public AppSettings AppSettings { get; set; }

    public string GithubUrl { get; set; }

    public string DogsUrl { get; set; }
}
