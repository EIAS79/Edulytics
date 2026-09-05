using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Edulytics.Web.Controllers;

[Route("contact")]
public sealed class ContactController : Controller
{
    [AllowAnonymous]
    [HttpGet("")]
    public IActionResult Index() => View();
}
