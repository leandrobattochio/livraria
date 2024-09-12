using Microsoft.Extensions.Logging;
using Meziantou.Extensions.Logging.Xunit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Livraria.IntegrationTests.Core;

public abstract class BaseIntegrationTest<TFixture>(TFixture fixture, ITestOutputHelper testOutputHelper)
    : IClassFixture<TFixture>
    where TFixture : BaseIntegrationFixture
{
    protected HttpClient CreateClient()
    {
        return fixture
            .WithWebHostBuilder(x =>
            {
                x.ConfigureLogging(b =>
                {
                    b.ClearProviders();
                    b.Services.AddSingleton<ILoggerProvider>(new XUnitLoggerProvider(testOutputHelper));
                });
            })
            .CreateClient();
    }
    
    
    protected HttpClient CreateClient(Action<WebHostBuilderContext, IConfigurationBuilder> configureDelegate)
    {
        return fixture
            .WithWebHostBuilder(x =>
            {
                x.ConfigureLogging(b =>
                {
                    b.ClearProviders();
                    b.Services.AddSingleton<ILoggerProvider>(new XUnitLoggerProvider(testOutputHelper));
                });

                x.ConfigureAppConfiguration(configureDelegate);
            })
            .CreateClient();
    }
}