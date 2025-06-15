using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebAppEF.AdventureS.Ef;

namespace WebAppEF.AdventureS.Controllers;

public class UsersController : ControllerBase
{
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly ILogger<UsersController> _logger;

    public UsersController(ApplicationDbContext applicationDbContext, ILogger<UsersController> logger)
    {
        _applicationDbContext = applicationDbContext;
        _logger = logger;
    }

    [HttpPost("users")]
    public async Task<IActionResult> CreateUserAsync([FromBody] UserRequest userRequest)
    {
        var userTypes = _applicationDbContext.Users.Include(x => x.UserTypeMappings).SelectMany(x => x.UserTypeMappings).Where(x => x.CreatedBy == "Creator").ToQueryString();
        var user = new User
        {
            UserGuid = Guid.NewGuid().ToString(),
            Name = userRequest.Name,
            CreatedAt = DateTime.UtcNow,
        };

        await _applicationDbContext.ExecuteAsync(async () => await _applicationDbContext.Users.AddAsync(user));

        var defaultUserType = _applicationDbContext.UserTypes.FirstOrDefault(x => x.UserTypeName == "Default Type");

        if (defaultUserType is null)
        {
            defaultUserType = new UserType
            {
                UserTypeName = "Default Type", // Assuming a default UserTypeName for demonstration
                UserTypeDescription = "Default user type description" // Assuming a default description for demonstration
            };

            await _applicationDbContext.ExecuteAsync(async () => await _applicationDbContext.UserTypes.AddAsync(defaultUserType));
        }

        await _applicationDbContext.ExecuteAsync(async () => await _applicationDbContext.UserTypeMappings.AddAsync(new UserTypeMapping
        {
            UserId = user.Id,
            UserTypeId = defaultUserType.Id,
            CreatedBy = userRequest.CreatedBy
        }));

        var userResponse = new UserResponse { Id = user.Id, UserGuid = user.UserGuid };
        return Ok(userResponse);
    }

    [HttpPost("roles")]
    public async Task<IActionResult> CreateRoleAsync([FromBody] RoleRequest roleRequest)
    {
        IDbContextTransaction transaction = await _applicationDbContext.Database.BeginTransactionAsync();

        var role = new Role
        {
            RoleType = roleRequest.RoleType.Value,
        };

        try
        {
            _applicationDbContext.Roles.Add(role);
            await _applicationDbContext.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch
        {
            transaction.Rollback();
            return BadRequest();
        }

        var roleResponse = new RoleResponse { Id = role.Id };
        return Ok(roleResponse);
    }

    [HttpPost("users/{userId}/roles/{roleId}/assign")]
    public async Task<IActionResult> AssignUserRoleAsync([FromRoute] int userId, int roleId)
    {
        var users = _applicationDbContext.Users.Include(x => x.Role).ToQueryString();
        int[] userIds = [1, 2, 3, 4, 5];

        var query = _applicationDbContext.Users.Where(x => userIds.Contains(x.Id)).ToQueryString();
        var result = _applicationDbContext.Users.Where(x => userIds.Contains(x.Id)).ToList();

        var user = _applicationDbContext.Users.FirstOrDefault(x => x.Id == userId);

        if (user == null)
        {
            return NotFound("User not found.");
        }
        var sql = _applicationDbContext.Roles.Where(x => x.Id == roleId).SelectMany(x => x.Users).AsSplitQuery().ToQueryString();

        var roleUsers = _applicationDbContext.Roles.Where(x => x.Id == roleId).SelectMany(x => x.Users).ToList();

        var role = _applicationDbContext.Roles.Where(x => x.Id == roleId).FirstOrDefault();

        if (role == null)
        {
            return NotFound("Role not found.");
        }

        user.RoleId = role.Id;

        await _applicationDbContext.SaveChangesAsync();

        return Ok();
    }

    public class RoleRequest
    {
        [Required]
        public RoleType? RoleType { get; set; }
    }

    public class UserRequest
    {
        public string Name { get; set; }

        public string CreatedBy { get; set; }
    }

    public class RoleResponse
    {
        public int Id { get; set; }
    }

    public class UserResponse
    {
        public int Id { get; set; }

        public string UserGuid { get; set; }
    }
}
