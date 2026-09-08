using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Edulytics.Web.Controllers;

[AllowAnonymous]
[Route("contact")]
public sealed class ContactController : Controller
{
    [HttpGet("")]
    public IActionResult Index() => View();

    [HttpGet("sales-enquiry")]
    public IActionResult SalesEnquiry() => View("Inquiry", "sales");

    [HttpGet("request-demo")]
    public IActionResult RequestDemo() => View("Inquiry", "demo");

    [HttpGet("help")]
    public IActionResult HelpAlias() => Redirect("/help");

    [HttpGet("/help")]
    public IActionResult Help() => View();
}
