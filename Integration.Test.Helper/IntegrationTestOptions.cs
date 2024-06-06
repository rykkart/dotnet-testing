using Microsoft.Extensions.DependencyInjection;

namespace Integration.Test.Helper;

public record IntegrationTestOptions
{
    internal string? RelativeContentRoot { get; private set; }
    internal string? Environment { get; private set; }
    internal readonly Dictionary<Type, object> Dependencies = new();
    internal Action<IServiceCollection>? ServiceCollection { get; private set; }
    internal bool AllowAutoRedirect { get; private set; } = true;
    internal List<KeyValuePair<string, string?>> Configuration { get; private set; } = [];

    public void Dependency(Action<IServiceCollection>? action = null) => ServiceCollection = action;

    public IntegrationTestOptions SetRelativeContentRoot(string? contentRoot)
    {
        RelativeContentRoot = contentRoot;
        return this;
    }

    public IntegrationTestOptions SetEnvironment(string? environment)
    {
        Environment = environment;
        return this;
    }

    public IntegrationTestOptions SetAllowAutoRedirect(bool allowAutoRedirect)
    {
        AllowAutoRedirect = allowAutoRedirect;
        return this;
    }

    public IntegrationTestOptions Dependency<T>(T instance)
    {
        if (instance != null)
            Dependencies.Add(typeof(T), instance);
        return this;
    }

    public IntegrationTestOptions UseConfiguration(params KeyValuePair<string, string?>[] config)
    {
        Configuration = config.ToList();
        return this;
    }
}