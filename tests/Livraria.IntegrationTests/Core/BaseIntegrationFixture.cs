namespace Livraria.IntegrationTests.Core;

public abstract class BaseIntegrationFixture :
    CustomWebApplicationFactory<Livraria.ApiHost.Program>,
    IAsyncLifetime
{
    protected virtual Task SeedInitialData() => Task.CompletedTask;
    
    public new async Task InitializeAsync()
    {
        await base.InitializeAsync();
        await SeedInitialData();
    }
    
    public new void Dispose()
    {
    }
}