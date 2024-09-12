using System.Data;
using Livraria.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Testcontainers.PostgreSql;

namespace Livraria.IntegrationTests.Core;

public class CustomWebApplicationFactory<TProgram>
    : WebApplicationFactory<TProgram>, 
        IAsyncLifetime 
    where TProgram : class
{
    private static readonly object _lock = new();
    
    private readonly PostgreSqlContainer _dbContainer =
        new PostgreSqlBuilder()
            .WithPassword("Senha126!A@")
            .WithAutoRemove(true)
            .Build();
    
    /// <summary>
    /// There is a problem with using Serilog's "CreateBootstrapLogger" when trying to initialize a web host.
    /// This is because in tests, multiple hosts are created in parallel, and Serilog's static logger is not thread-safe.
    /// The way around this without touching the host code is to lock the creation of the host to a single thread at a time.
    /// https://github.com/serilog/serilog-aspnetcore/issues/289#issuecomment-2085267457
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    protected override IHost CreateHost(IHostBuilder builder)
    {
        lock(_lock)
            return base.CreateHost(builder);
    }

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();
    }

    public new async Task DisposeAsync()
    {
        await _dbContainer.StopAsync();
    }
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        // override appsettings for RabbitMQ
        builder.ConfigureAppConfiguration((c, b) =>
        {
            b.Sources.Clear();
            b.AddInMemoryCollection(
                new Dictionary<string, string>
                {
                    ["ConnectionStrings:PostgreDatabase"] = _dbContainer.GetConnectionString()
                }!);
            
            b.AddJsonFile($"appsettings.Testing.json", optional: false, reloadOnChange: false)
                .AddEnvironmentVariables();
        });
        
        // override EntityFramework
        builder.ConfigureServices(services =>
        {
            var dbContextDescriptor = services.SingleOrDefault(
                d => d.ServiceType ==
                     typeof(DbContextOptions<LivrariaDbContext>));

            if(dbContextDescriptor != null)
                services.Remove(dbContextDescriptor);

            var dbConnectionDescriptor = services.SingleOrDefault(
                d => d.ServiceType ==
                     typeof(IDbConnection));

            if(dbConnectionDescriptor != null)
                services.Remove(dbConnectionDescriptor);

            services.AddDbContext<LivrariaDbContext>((container, options) =>
            {
                options.UseNpgsql(_dbContainer.GetConnectionString());
            });
        });

        builder.UseEnvironment("Testing");
    }
}