

using Microsoft.AspNetCore.Builder;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System.Data;
using System.Text.Json.Serialization;
using System.Text.Json;
using WebAppEF.AdventureS.Infrastructure;
using WebAppEF.AdventureS.Interfaces;
using WebAppEF.AdventureS.Services;
using System;
using Microsoft.EntityFrameworkCore;
using WebAppEF.AdventureS.Ef;
using WebAppEF.AdventureS.Controllers;

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

        builder.Services.AddDbContext<ApplicationDbContext>(
            x => x.UseSqlServer(builder.Configuration["DefaultConnection"]));

        builder.Services.AddScoped<ITransactionalDbContext, DefaultTransactionDbContext>();

        builder.Services.AddScoped<IDbConnection>(sp =>
        {
            var config = sp.GetRequiredService<IOptions<ConfigurationOptions>>();
            var connectionString = config.Value.ConnectionString;
            return new SqlConnection(connectionString);
        });


        builder.Services.AddScoped<IWorkOrderRepository, WorkOrderRepository>();

        builder.Services.AddControllers().AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
            options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            options.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
            options.JsonSerializerOptions.ReadCommentHandling = JsonCommentHandling.Skip;
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });


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
