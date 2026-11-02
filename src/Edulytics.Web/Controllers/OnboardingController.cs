using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Edulytics.Web.Controllers;

[Route("request-demo")]
public sealed class OnboardingController : Controller
{
    [HttpGet("")]
    [AllowAnonymous]
    public IActionResult Index() =>
        Redirect("/contact/request-demo");

    [HttpPost("")]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public IActionResult SubmitLegacyDemo() =>
        Redirect("/contact/request-demo");

    [HttpGet("thanks")]
    [AllowAnonymous]
    public IActionResult Thanks() =>
        Redirect("/contact/request-demo");
}
