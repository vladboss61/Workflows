namespace DockerContainersTests;

using System.Threading.Tasks;
using Xunit;
using Testcontainers.PostgreSql;

public class PostgresTest : IAsyncLifetime
{
    private readonly PostgreSqlContainer _postgresContainer;

    public PostgresTest()
    {
    }

    public async Task InitializeAsync()
    {

    }

    public async Task DisposeAsync()
    {
    }

    [Fact]
    public async Task Should_Insert_And_Select_User1()
    {

    }

    [Fact]
    public async Task Should_Insert_And_Select_User2()
    {

    }
}
