

using Microsoft.AspNetCore.Builder;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System.Data;
using WebAppEF.AdventureS.Infrastructure;

namespace WebAppEF.AdventureS;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Configuration
            .AddJsonFile("appsettings.Development.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables();

        builder.Services.Configure<ConfigurationOptions>(builder.Configuration.GetSection(nameof(ConfigurationOptions)));

        builder.Services.AddScoped<IDbConnection>(sp =>
        {
            var config = sp.GetRequiredService<IOptions<ConfigurationOptions>>();
            var connectionString = config.Value.ConnectionString;
            return new SqlConnection(connectionString);
        });

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}
