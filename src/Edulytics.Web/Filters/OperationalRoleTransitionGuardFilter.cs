using System.Security.Claims;
using Edulytics.Core.Constants;
using Edulytics.Services.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Localization;

namespace Edulytics.Web.Filters;

/// <summary>
/// Keeps Teacher and Student as distinct operational account types on the
/// ordinary Change Role endpoint. Student creation has its own provisioning
/// workflow because it also creates the student profile and class enrollment.
/// </summary>
public sealed class OperationalRoleTransitionGuardFilter
    : IAsyncActionFilter
{
    private readonly ISchoolUserManagementService _users;
    private readonly IStringLocalizer<PlatformResource> _text;

    public OperationalRoleTransitionGuardFilter(
        ISchoolUserManagementService users,
        IStringLocalizer<PlatformResource> text)
    {
        _users = users;
        _text = text;
    }

    public async Task OnActionExecutionAsync(
        ActionExecutingContext context,
        ActionExecutionDelegate next)
    {
        if (!TryGetChangeRoleRequest(
                context,
                out var userId,
                out var schoolId,
                out var requestedRole))
        {
            await next();
            return;
        }

        if (context.Controller is not Controller controller ||
            !Guid.TryParse(
                controller.User.FindFirstValue(
                    ClaimTypes.NameIdentifier),
                out var actorUserId))
        {
            context.Result = new ForbidResult();
            return;
        }

        var target = await _users.GetAsync(
            actorUserId,
            schoolId,
            userId,
            context.HttpContext.RequestAborted);

        if (target.Value is null)
        {
            // Preserve the controller's existing not-found/access-denied
            // handling when the target cannot be resolved.
            await next();
            return;
        }

        if (IsBlockedOperationalTransition(
                target.Value.Role,
                requestedRole))
        {
            controller.TempData["SchoolUserError"] =
                _text["UserInvalidRole"].Value;

            context.Result =
                new RedirectToActionResult(
                    "Details",
                    "SchoolUsers",
                    new
                    {
                        id = userId,
                        schoolId
                    });

            return;
        }

        await next();
    }

    private static bool TryGetChangeRoleRequest(
        ActionExecutingContext context,
        out Guid userId,
        out Guid schoolId,
        out string requestedRole)
    {
        userId = Guid.Empty;
        schoolId = Guid.Empty;
        requestedRole = string.Empty;

        if (context.ActionDescriptor is not
            ControllerActionDescriptor action ||
            !string.Equals(
                action.ControllerName,
                "SchoolUsers",
                StringComparison.Ordinal) ||
            !string.Equals(
                action.ActionName,
                "ChangeRole",
                StringComparison.Ordinal) ||
            !string.Equals(
                context.HttpContext.Request.Method,
                "POST",
                StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        if (!context.ActionArguments.TryGetValue(
                "id",
                out var idValue) ||
            idValue is not Guid id ||
            !context.ActionArguments.TryGetValue(
                "schoolId",
                out var schoolValue) ||
            schoolValue is not Guid school ||
            !context.ActionArguments.TryGetValue(
                "role",
                out var roleValue) ||
            roleValue is not string role)
        {
            return false;
        }

        userId = id;
        schoolId = school;
        requestedRole = role.Trim();
        return true;
    }

    private static bool IsBlockedOperationalTransition(
        string currentRole,
        string requestedRole) =>
        (
            string.Equals(
                currentRole,
                RoleNames.Teacher,
                StringComparison.Ordinal) &&
            string.Equals(
                requestedRole,
                RoleNames.Student,
                StringComparison.Ordinal)
        ) ||
        (
            string.Equals(
                currentRole,
                RoleNames.Student,
                StringComparison.Ordinal) &&
            string.Equals(
                requestedRole,
                RoleNames.Teacher,
                StringComparison.Ordinal)
        );
}
