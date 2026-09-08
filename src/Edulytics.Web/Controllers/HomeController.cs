using System.Diagnostics;
using Edulytics.Web.Localization;
using Edulytics.Web.Models;
using Edulytics.Core.Curriculum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Edulytics.Web.Controllers;

public sealed class HomeController : Controller
{
    private static readonly IReadOnlyDictionary<string, (string En, string Pl)> MarketingPages =
        new Dictionary<string, (string En, string Pl)>(StringComparer.OrdinalIgnoreCase)
        {
            ["product/results-backed-by-data"] = ("Results Backed by Data", "Wyniki potwierdzone danymi"),
            ["product/support-you-can-rely-on"] = ("Support You Can Rely On", "Wsparcie, na którym możesz polegać"),
            ["product/student-portal"] = ("Student Portal", "Portal ucznia"),
            ["product/assessment-and-practice"] = ("Assessment & Practice", "Ocenianie i ćwiczenia"),
            ["product/mastery-and-next-step"] = ("Mastery & Next Step", "Opanowanie i kolejny krok"),
            ["product/mathematics"] = ("Mathematics", "Matematyka"),
            ["product/curricula"] = ("Curricula & Learning Outcomes", "Programy nauczania i efekty uczenia się"),
            ["product/features"] = ("Product Features", "Funkcje produktu"),
            ["product/edulytics-ai"] = ("Edulytics AI", "Edulytics AI"),
            ["product/languages"] = ("Multilingual Editions", "Wersje wielojęzyczne"),
            ["product/technical-requirements"] = ("Technical Requirements", "Wymagania techniczne"),
            ["teachers/overview"] = ("Edulytics for Teachers", "Edulytics dla nauczycieli"),
            ["teachers/assessment-and-curriculum"] = ("Assessment & Curriculum for Teachers", "Ocenianie i program nauczania dla nauczycieli"),
            ["parents/overview"] = ("Edulytics for Home", "Edulytics w domu"),
            ["schools/overview"] = ("Edulytics for Education Leaders", "Edulytics dla liderów edukacji"),
            ["students/overview"] = ("Edulytics for Students", "Edulytics dla uczniów"),
            ["company/partnerships"] = ("Partnerships", "Partnerstwa"),
            ["company/about"] = ("About Edulytics", "O Edulytics")
        };

    [AllowAnonymous]
    [HttpGet("/")]
    public IActionResult Index()
    {
        if (!CultureCookie.TryRead(Request, out _))
        {
            Response.Cookies.Append(
                CultureCookie.Name,
                CultureCookie.CreateValue("pl"),
                new CookieOptions
                {
                    Path = "/",
                    Expires = DateTimeOffset.UtcNow.AddYears(1),
                    IsEssential = true,
                    HttpOnly = true,
                    SameSite = SameSiteMode.Strict,
                    Secure = Request.IsHttps
                });
        }

        return View();
    }

    [AllowAnonymous]
    [HttpGet("/product/learning-built-for-understanding")]
    public IActionResult LearningBuiltForUnderstanding()
    {
        return View();
    }

    [AllowAnonymous]
    [HttpGet("/product")]
    public IActionResult ProductLanding()
    {
        return Redirect("/product/features");
    }

    [AllowAnonymous]
    [HttpGet("/teachers")]
    public IActionResult TeachersLanding()
    {
        return Redirect("/teachers/overview");
    }

    [AllowAnonymous]
    [HttpGet("/parents")]
    public IActionResult ParentsLanding()
    {
        return Redirect("/parents/overview");
    }

    [AllowAnonymous]
    [HttpGet("/schools")]
    public IActionResult SchoolsLanding()
    {
        return Redirect("/schools/overview");
    }

    [AllowAnonymous]
    [HttpGet("/students")]
    public IActionResult StudentsLanding()
    {
        return Redirect("/students/overview");
    }

    [AllowAnonymous]
    [HttpGet("/product/{slug}")]
    public IActionResult ProductPage(string slug)
    {
        return MarketingPage("product", slug);
    }

    [AllowAnonymous]
    [HttpGet("/teachers/{slug}")]
    public IActionResult TeacherPage(string slug)
    {
        return MarketingPage("teachers", slug);
    }

    [AllowAnonymous]
    [HttpGet("/parents/{slug}")]
    public IActionResult ParentPage(string slug)
    {
        return MarketingPage("parents", slug);
    }

    [AllowAnonymous]
    [HttpGet("/schools/{slug}")]
    public IActionResult SchoolPage(string slug)
    {
        return MarketingPage("schools", slug);
    }

    [AllowAnonymous]
    [HttpGet("/students/{slug}")]
    public IActionResult StudentPage(string slug)
    {
        return MarketingPage("students", slug);
    }

    [AllowAnonymous]
    [HttpGet("/company/{slug}")]
    public IActionResult CompanyPage(string slug)
    {
        return MarketingPage("company", slug);
    }

    [AllowAnonymous]
    [HttpGet("/legal/content-sources")]
    public IActionResult ContentSources()
    {
        return View(MathematicsCurriculumPackRegistry.All);
    }

    [AllowAnonymous]
    [HttpPost("/set-culture")]
    [ValidateAntiForgeryToken]
    public IActionResult SetCulture(string? culture, string? returnUrl)
    {
        if (!CultureCookie.IsSupported(culture))
        {
            Response.Cookies.Delete(
                CultureCookie.Name,
                new CookieOptions
                {
                    Path = "/",
                    SameSite = SameSiteMode.Strict,
                    Secure = Request.IsHttps
                });

            return RedirectToAction(nameof(Index));
        }

        Response.Cookies.Append(
            CultureCookie.Name,
            CultureCookie.CreateValue(culture!),
            new CookieOptions
            {
                Path = "/",
                Expires = DateTimeOffset.UtcNow.AddYears(1),
                IsEssential = true,
                HttpOnly = true,
                SameSite = SameSiteMode.Strict,
                Secure = Request.IsHttps
            });

        if (!string.IsNullOrWhiteSpace(returnUrl) &&
            Url.IsLocalUrl(returnUrl))
        {
            return LocalRedirect(returnUrl);
        }

        return RedirectToAction("Login", "Account");
    }

    [Authorize]
    [HttpGet("/access-denied")]
    public IActionResult AccessDenied()
    {
        return View();
    }

    [AllowAnonymous]
    [ResponseCache(
        Duration = 0,
        Location = ResponseCacheLocation.None,
        NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel
        {
            RequestId =
                Activity.Current?.Id ??
                HttpContext.TraceIdentifier
        });
    }

    private IActionResult MarketingPage(string section, string slug)
    {
        var key = $"{section}/{slug}";
        if (!MarketingPages.TryGetValue(key, out var title))
        {
            return NotFound();
        }

        ViewData["MarketingPageKey"] = key;
        ViewData["MarketingPageUrl"] = $"/{key}";
        ViewData["MarketingPageTitle"] = title.En;
        ViewData["MarketingPageTitlePl"] = title.Pl;
        return View("PublicContentPage");
    }
}
