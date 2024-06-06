// ReSharper disable NullableWarningSuppressionIsUsed

using static Integration.Test.Helper.IntegrationTestEnvironmentExtensions;

namespace Integration.Test.Helper.Tests;

public class IntegrationTestEnvironmentTests
{
    [Test]
    public async Task Should_handle_request()
    {
        var result = await CreateHttpClient<TestProgram>().GetAsync("test");
        var response = (await result.Content.ReadFromJsonAsync<Response>())!;

        result.IsSuccessStatusCode.Should().BeTrue();
        response.Name.Should().Be("Dr Alban");
    }

    [Test]
    public async Task Should_be_able_to_replace_services()
    {
        var result = await CreateHttpClient<TestProgram>(x => x.Dependency<MyUnicorn>(new MyMock())).GetAsync("test");
        var response = await result.Content.ReadFromJsonAsync<Response>();

        result.IsSuccessStatusCode.Should().BeTrue();
        response!.Name.Should().Be("Slim shady");
    }

    [TestCase("http://redirect.url/")]
    public async Task Should_follow_redirects(string redirectUrl)
    {
        var result = await CreateHttpClient<TestProgram>()
            .GetAsync($"redirect?redirectUrl={redirectUrl}");

        result.RequestMessage!.RequestUri!.Should().Be(redirectUrl);
    }

    [TestCase("http://redirect.url/")]
    public async Task Should_not_follow_redirects(string redirectUrl)
    {
        var result = await CreateHttpClient<TestProgram>(x => x.SetAllowAutoRedirect(false))
            .GetAsync($"redirect?redirectUrl={redirectUrl}");

        result.StatusCode.Should().Be(HttpStatusCode.Redirect);
        result.Headers.Location!.Should().Be(redirectUrl);
    }
}

public record Response(string Name);

public class MyMock : MyUnicorn
{
    public override string MyName => "Slim shady";
}