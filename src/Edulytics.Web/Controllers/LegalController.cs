using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Edulytics.Web.Controllers;

[AllowAnonymous]
public sealed class LegalController : Controller
{
    [HttpGet("/legal/privacy")]
    public IActionResult Privacy() => View();

    [HttpGet("/legal/terms")]
    public IActionResult Terms() => View();

    [HttpGet("/legal/data-processing-agreement")]
    public IActionResult DataProcessingAgreement() => View();
}
