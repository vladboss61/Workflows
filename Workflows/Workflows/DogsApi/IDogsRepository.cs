using Refit;
using System.Threading.Tasks;
using WorkflowsEx.DogsApi.Models;

namespace WorkflowsEx.DogsApi;

public interface IDogsRepository
{
    [Get("/api/breeds/image/random")]
    public Task<DogRandomResponse> GetRandomDogAsync();
}
