using System.Security.Claims;
using Edulytics.Core.Curriculum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Edulytics.Web.Controllers;

/// <summary>
/// Private developer research index. This is not a public CC attribution
/// mechanism and must not be used to certify a copied asset's public display.
/// </summary>
[Authorize]
[Route("internal/developer/credits")]
public sealed class DeveloperCreditsController : Controller
{
    private readonly IConfiguration _configuration;

    public DeveloperCreditsController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    [HttpGet("")]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Index()
    {
        // Never grant all SuperAdmins, schools, teachers or students access.
        // An explicit single designer's stable user ID is required. Unset or
        // malformed configuration must deny ALL accounts (fail closed).
        if (!Guid.TryParse(_configuration["DeveloperCredits:UserId"], out var ownerId) ||
            !Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var actorId) ||
            actorId != ownerId ||
            User.Identity?.IsAuthenticated != true)
        {
            return NotFound();
        }

        Response.Headers["X-Robots-Tag"] = "noindex, nofollow, noarchive";
        Response.Headers.CacheControl = "private, no-store, max-age=0";
        return View(MathematicsCurriculumPackRegistry.All);
    }
}
