using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Integration.Test.Helper;

public class IntegrationTestEnvironment<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
{
    readonly IntegrationTestOptions _options = new();

    public IntegrationTestEnvironment(Action<IntegrationTestOptions>? with = null) => with?.Invoke(_options);

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        if (!string.IsNullOrWhiteSpace(_options.RelativeContentRoot))
            builder.UseSolutionRelativeContentRoot(_options.RelativeContentRoot);
        if (!string.IsNullOrWhiteSpace(_options.Environment))
            builder.UseEnvironment(_options.Environment);

        builder.ConfigureTestServices(services =>
        {
            foreach (var (type, value) in _options.Dependencies)
                services.AddSingleton(type, value);
            _options.ServiceCollection?.Invoke(services);
        });

        if (_options.Configuration.Count != 0)
        {
            var testConfigurationBuilder =
                new ConfigurationBuilder()
                    .AddInMemoryCollection(_options.Configuration)
                    .Build();
            builder.UseConfiguration(testConfigurationBuilder);
        }

        base.ConfigureWebHost(builder);
    }

    public HttpClient GetHttpClient => CreateClient(
        new WebApplicationFactoryClientOptions { AllowAutoRedirect = _options.AllowAutoRedirect });
}