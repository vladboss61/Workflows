namespace WorkflowsEx.DogsApi.Models;

public class DogRandomResponse
{
    public string Message { get; set; }

    public string Status { get; set; }

    public override string ToString()
    {
        return $"[Message]: {Message} [Status]: {Status}";
    }
}
