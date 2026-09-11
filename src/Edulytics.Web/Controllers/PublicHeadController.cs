using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Edulytics.Web.Controllers;

[AllowAnonymous]
public sealed class PublicHeadController : Controller
{
    [HttpHead("/")]
    public IActionResult Home() => Ok();
}
