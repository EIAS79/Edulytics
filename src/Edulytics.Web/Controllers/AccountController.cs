using Edulytics.Core.Constants;
using System.Globalization;
using Edulytics.Data.Identity;
using Edulytics.Services.Users;
using Edulytics.Web.Localization;
using Edulytics.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Localization;

namespace Edulytics.Web.Controllers;

public sealed class AccountController : Controller
{
    private static readonly HashSet<string>
        PublicAccountTypes =
        new(
            [
                RoleNames.SchoolAdmin,
                RoleNames.SubjectSupervisor,
                RoleNames.Teacher,
                RoleNames.Student
            ],
            StringComparer.Ordinal);

    private readonly SignInManager<ApplicationUser>
        _signInManager;

    private readonly UserManager<ApplicationUser>
        _userManager;

    private readonly ISchoolUserManagementService
        _schoolUsers;

    private readonly IStringLocalizer<PlatformResource>
        _text;

    public AccountController(
        SignInManager<ApplicationUser> signInManager,
        UserManager<ApplicationUser> userManager,
        ISchoolUserManagementService schoolUsers,
        IStringLocalizer<PlatformResource> text)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _schoolUsers = schoolUsers;
        _text = text;
    }

    [AllowAnonymous]
    [HttpGet("/account/login")]
    public IActionResult Login(
        string? returnUrl = null)
    {
        if (!CultureCookie.TryRead(Request, out _))
        {
            return RedirectToAction(
                "Index",
                "Home");
        }

        ViewData["ReturnUrl"] =
            returnUrl;

        return View(
            new LoginViewModel());
    }

    [AllowAnonymous]
    [HttpPost("/account/login")]
    [EnableRateLimiting("Login")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(
        LoginViewModel model,
        string? returnUrl = null)
    {
        if (!CultureCookie.TryRead(Request, out _))
        {
            return RedirectToAction(
                "Index",
                "Home");
        }

        ViewData["ReturnUrl"] =
            returnUrl;

        if (!string.IsNullOrWhiteSpace(model.AccountType) &&
            !IsSupportedAccountType(model.AccountType))
        {
            AddAccountTypeRequired();
        }

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var user =
            await _userManager.FindByEmailAsync(
                model.Email!);

        if (user is null)
        {
            AddInvalidCredentials();
            return View(model);
        }

        // Check the credential first without creating an authentication cookie.
        // The selected account type is validated only after the password is
        // known to be correct, preventing role information from being exposed
        // for accounts an attacker cannot authenticate.
        var credentialResult =
            await _signInManager
                .CheckPasswordSignInAsync(
                    user,
                    model.Password!,
                    lockoutOnFailure: true);

        if (!credentialResult.Succeeded)
        {
            AddInvalidCredentials();
            return View(model);
        }

        var access =
            await _schoolUsers
                .EvaluateSignInAsync(user.Id);

        if (!access.Allowed)
        {
            AddInvalidCredentials();
            return View(model);
        }

        // Platform administrators remain an internal exception: they are not
        // exposed as a fifth public account type. School users must explicitly
        // select one of the four public account types and it must match the
        // role registered for the authenticated account.
        if (!access.IsPlatformAdministrator)
        {
            if (!IsSupportedAccountType(model.AccountType))
            {
                AddAccountTypeRequired();
                return View(model);
            }

            if (!string.Equals(
                    access.Role,
                    model.AccountType,
                    StringComparison.Ordinal))
            {
                AddAccountTypeMismatch(model.AccountType!);
                return View(model);
            }
        }

        await _signInManager.SignInAsync(
            user,
            isPersistent: false);

        if (Url.IsLocalUrl(returnUrl))
        {
            return Redirect(returnUrl!);
        }

        if (access.IsPlatformAdministrator)
        {
            return RedirectToAction(
                "Dashboard",
                "Platform");
        }

        if (string.Equals(
                access.Role,
                RoleNames.Student,
                StringComparison.Ordinal))
        {
            return RedirectToAction(
                "Dashboard",
                "StudentPortal");
        }

        return RedirectToAction(
            "Dashboard",
            "SchoolHome");
    }

    [AllowAnonymous]
    [HttpGet("/account/set-password")]
    public IActionResult SetPassword(
        Guid userId,
        string token,
        string culture)
    {
        culture = ApplySetupCulture(culture);

        if (userId == Guid.Empty ||
            string.IsNullOrWhiteSpace(token))
        {
            return View(
                new SetPasswordViewModel
                {
                    UserId = userId,
                    Token = token ?? string.Empty,
                    Culture = culture
                });
        }

        return View(
            new SetPasswordViewModel
            {
                UserId = userId,
                Token = token,
                Culture = culture
            });
    }

    [AllowAnonymous]
    [HttpPost("/account/set-password")]
    [IgnoreAntiforgeryToken]
    [EnableRateLimiting("PasswordSetup")]
    public async Task<IActionResult> SetPassword(
        SetPasswordViewModel model,
        CancellationToken cancellationToken)
    {
        model.Culture =
            ApplySetupCulture(model.Culture);

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var result =
            await _schoolUsers
                .CompletePasswordSetupAsync(
                    model.UserId,
                    model.Token,
                    model.Password,
                    cancellationToken);

        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(
                    string.Empty,
                    _text[
                        error.Code.ToString()
                    ].Value);
            }

            return View(model);
        }

        return RedirectToAction(
            nameof(PasswordSet),
            new
            {
                culture = model.Culture
            });
    }

    [AllowAnonymous]
    [HttpGet("/account/password-set")]
    public IActionResult PasswordSet(
        string? culture)
    {
        ApplySetupCulture(culture);

        return View();
    }

    [HttpPost("/account/logout")]
    [ValidateAntiForgeryToken]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();

        return RedirectToAction(
            "Index",
            "Home");
    }

    private void AddInvalidCredentials()
    {
        ModelState.AddModelError(
            string.Empty,
            _text["InvalidCredentials"].Value);
    }

    private void AddAccountTypeRequired()
    {
        ModelState.AddModelError(
            nameof(LoginViewModel.AccountType),
            IsPolishUi()
                ? "Najpierw wybierz typ konta."
                : "Choose your account type first.");
    }

    private void AddAccountTypeMismatch(
        string accountType)
    {
        var label = GetAccountTypeLabel(accountType);

        ModelState.AddModelError(
            nameof(LoginViewModel.AccountType),
            IsPolishUi()
                ? $"To konto nie jest zarejestrowane jako {label}. Wybierz właściwy typ konta."
                : $"This account is not registered as {label}. Please choose the correct account type.");
    }

    private bool IsPolishUi() =>
        string.Equals(
            CultureInfo.CurrentUICulture.TwoLetterISOLanguageName,
            "pl",
            StringComparison.OrdinalIgnoreCase);

    private string GetAccountTypeLabel(
        string accountType)
    {
        if (IsPolishUi())
        {
            return accountType switch
            {
                RoleNames.SchoolAdmin => "Administrator szkoły",
                RoleNames.SubjectSupervisor => "Opiekun przedmiotu",
                RoleNames.Teacher => "Nauczyciel",
                RoleNames.Student => "Uczeń",
                _ => "wybrany typ konta"
            };
        }

        return accountType switch
        {
            RoleNames.SchoolAdmin => "a School Administrator",
            RoleNames.SubjectSupervisor => "a Subject Supervisor",
            RoleNames.Teacher => "a Teacher",
            RoleNames.Student => "a Student",
            _ => "the selected account type"
        };
    }

    private static bool IsSupportedAccountType(
        string? accountType) =>
        !string.IsNullOrWhiteSpace(accountType) &&
        PublicAccountTypes.Contains(accountType);

    private string ApplySetupCulture(
        string? culture)
    {
        culture =
            culture?.Trim().ToLowerInvariant()
            ?? string.Empty;

        if (!CultureCookie.IsSupported(culture))
        {
            culture =
                CultureCookie.TryRead(
                    Request,
                    out var cookieCulture)
                    ? cookieCulture
                    : "en";
        }

        var cultureInfo =
            CultureInfo.GetCultureInfo(culture);

        CultureInfo.CurrentCulture =
            cultureInfo;

        CultureInfo.CurrentUICulture =
            cultureInfo;

        Response.Cookies.Append(
            CultureCookie.Name,
            CultureCookie.CreateValue(culture),
            new CookieOptions
            {
                Path = "/",
                Expires =
                    DateTimeOffset.UtcNow
                        .AddYears(1),
                IsEssential = true,
                HttpOnly = true,
                SameSite =
                    SameSiteMode.Strict,
                Secure =
                    Request.IsHttps
            });

        return culture;
    }
}
