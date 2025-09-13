using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dependency.ConsoleApp.Decorators;

public interface IUserRepository
{
    public void ChangeUserAsync();
}

public class UserRepository : IUserRepository
{
    public void ChangeUserAsync()
    {
        // Example.
        //throw new InvalidOperationException("User changed in wrong way.");

        Console.WriteLine("User changing logic.");
    }
}

public sealed class RetryDecoratorUserRepository : IUserRepository
{
    private readonly IUserRepository _userRepository;

    public RetryDecoratorUserRepository(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public void ChangeUserAsync()
    {
        int attempts = 1;
        int retryMaxAttempts = 5;
        
        while (attempts <= retryMaxAttempts)
        {
            Console.WriteLine($"Attempt: {attempts}");

            try
            {
                _userRepository.ChangeUserAsync();
                return;
            }
            catch (Exception _)
            {
               attempts++;
               Task.Delay(1000);
            }
        }
    }
}
