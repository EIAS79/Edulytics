using System.Security.Claims;
using Edulytics.Core.Constants;
using Edulytics.Services.DirectStudents;
using Edulytics.Web.ViewModels.DirectStudent;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Edulytics.Web.Controllers;

public sealed class DirectStudentController : Controller
{
    private readonly IDirectStudentRegistrationService _directStudents;
    private readonly IConfiguration _configuration;

    public DirectStudentController(
        IDirectStudentRegistrationService directStudents,
        IConfiguration configuration)
    {
        _directStudents = directStudents;
        _configuration = configuration;
    }

    [AllowAnonymous]
    [HttpGet("/student/join")]
    public IActionResult Join()
    {
        if (!RegistrationEnabled())
            return NotFound();

        return View(new DirectStudentRegistrationViewModel());
    }

    [AllowAnonymous]
    [HttpPost("/student/join")]
    [ValidateAntiForgeryToken]
    [EnableRateLimiting("Login")]
    public async Task<IActionResult> Join(
        DirectStudentRegistrationViewModel model,
        CancellationToken cancellationToken)
    {
        if (!RegistrationEnabled())
            return NotFound();

        if (!ModelState.IsValid)
            return View(model);

        var result = await _directStudents.RegisterAsync(
            new RegisterDirectStudentRequest(
                model.Email,
                model.Password,
                model.FirstName,
                model.LastName),
            cancellationToken);

        if (!result.Succeeded)
        {
            var message = result.Error switch
            {
                DirectStudentRegistrationErrorCode.InvalidEmail =>
                    "Enter a valid email address.",
                DirectStudentRegistrationErrorCode.DuplicateEmail =>
                    "An account already exists for this email address.",
                DirectStudentRegistrationErrorCode.PasswordPolicy =>
                    result.Details.Count > 0
                        ? string.Join(" ", result.Details)
                        : "The password does not meet the security requirements.",
                DirectStudentRegistrationErrorCode.Required =>
                    "Complete all required fields.",
                _ =>
                    "We could not create the account. Please try again."
            };

            ModelState.AddModelError(string.Empty, message);
            return View(model);
        }

        return RedirectToAction(
            "Login",
            "Account",
            new { direct = true });
    }

    [Authorize(Roles = RoleNames.Student)]
    [HttpGet("/direct-student")]
    public async Task<IActionResult> Dashboard(
        CancellationToken cancellationToken)
    {
        if (!TryUserId(out var userId) ||
            !await _directStudents.IsDirectStudentAsync(userId, cancellationToken))
        {
            return Forbid();
        }

        return View();
    }

    private bool RegistrationEnabled() =>
        _configuration.GetValue<bool>(
            "Edulytics:Features:DirectStudentRegistrationEnabled");

    private bool TryUserId(out Guid userId) =>
        Guid.TryParse(
            User.FindFirstValue(ClaimTypes.NameIdentifier),
            out userId);
}
