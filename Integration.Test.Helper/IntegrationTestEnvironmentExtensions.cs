namespace Integration.Test.Helper;

public static class IntegrationTestEnvironmentExtensions
{
    public static IntegrationTestEnvironment<TStartup> CreateEnvironment<TStartup>()
        where TStartup : class => new();

    public static IntegrationTestEnvironment<TStartup> CreateEnvironment<TStartup>(Action<IntegrationTestOptions> with)
        where TStartup : class => new(with);


    public static HttpClient CreateHttpClient<TStartup>()
        where TStartup : class => new IntegrationTestEnvironment<TStartup>().GetHttpClient;

    public static HttpClient CreateHttpClient<TStartup>(Action<IntegrationTestOptions> with)
        where TStartup : class => new IntegrationTestEnvironment<TStartup>(with).GetHttpClient;
}