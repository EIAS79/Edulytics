using System.Security.Claims;
using Edulytics.Web.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Edulytics.Tests.Security;

public sealed class DeveloperCreditsAuthorizationTests
{
    private static readonly Guid Developer = Guid.Parse("11111111-1111-4111-8111-111111111111");
    private static readonly Guid Other = Guid.Parse("22222222-2222-4222-8222-222222222222");

    private static DeveloperCreditsController Create(Guid? signedInAs, string? configuredDeveloper)
    {
        var configuration = new ConfigurationBuilder().AddInMemoryCollection(
            new Dictionary<string, string?>
            {
                ["DeveloperCredits:UserId"] = configuredDeveloper
            }).Build();

        var claims = signedInAs.HasValue
            ? new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, signedInAs.Value.ToString()),
                new Claim(ClaimTypes.Role, "SuperAdmin")
            }, authenticationType: "Test")
            : new ClaimsIdentity();

        return new DeveloperCreditsController(configuration)
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(claims)
                }
            }
        };
    }

    [Fact]
    public void RouteRequiresAuthentication()
    {
        var attribute = (AuthorizeAttribute?)Attribute.GetCustomAttribute(
            typeof(DeveloperCreditsController), typeof(AuthorizeAttribute));
        Assert.NotNull(attribute);
    }

    [Fact]
    public void NoConfiguredDesignerIsFailClosed()
    {
        Assert.IsType<NotFoundResult>(Create(Developer, null).Index());
        Assert.IsType<NotFoundResult>(Create(Developer, "invalid-id").Index());
    }

    [Fact]
    public void UnauthenticatedRequestCannotAccessDesignerRegistry()
    {
        Assert.IsType<NotFoundResult>(Create(null, Developer.ToString()).Index());
    }

    [Fact]
    public void EvenOtherSuperAdminsCannotReadDesignerRegistry()
    {
        Assert.IsType<NotFoundResult>(Create(Other, Developer.ToString()).Index());
    }

    [Fact]
    public void ExplicitMatchingDesignerCanViewRegistry()
    {
        var controller = Create(Developer, Developer.ToString());
        Assert.IsType<ViewResult>(controller.Index());
        Assert.Equal("noindex, nofollow, noarchive",
            controller.Response.Headers["X-Robots-Tag"].ToString());
        Assert.Contains("no-store",
            controller.Response.Headers.CacheControl.ToString());
    }
}
