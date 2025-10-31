using Integration.Test.Helper.Tests;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<MyUnicorn>();

var app = builder.Build();

app.MapGet("test", ([FromServices] MyUnicorn unicorn) => Results.Ok(new { name = unicorn.MyName }));
app.MapGet("redirect", ([FromQuery] string redirectUrl) => Results.Redirect(redirectUrl));
app.Run();

namespace Integration.Test.Helper.Tests
{
    public class TestProgram;

    public class MyUnicorn
    {
        public virtual string MyName => "Dr Alban";
    }
}