using System.Globalization;
using System.Security.Claims;
using Edulytics.Core.Constants;
using Edulytics.Core.Interfaces;
using Edulytics.Services.StudentSetup;
using Edulytics.Services.Users;
using Edulytics.Web.Email;
using Edulytics.Web.ViewModels.SchoolUsers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Localization;

namespace Edulytics.Web.Filters;

public sealed class DirectStudentCreationFilter
    : IAsyncActionFilter
{
    private readonly ISchoolUserManagementService _users;
    private readonly IStudentRoleProvisioningService _studentProvisioning;
    private readonly IStudentCreationClassCatalog _classes;
    private readonly IApplicationTransactionManager _transactions;
    private readonly IUserInvitationDeliveryService _invitations;
    private readonly IStringLocalizer<PlatformResource> _text;

    public DirectStudentCreationFilter(
        ISchoolUserManagementService users,
        IStudentRoleProvisioningService studentProvisioning,
        IStudentCreationClassCatalog classes,
        IApplicationTransactionManager transactions,
        IUserInvitationDeliveryService invitations,
        IStringLocalizer<PlatformResource> text)
    {
        _users = users;
        _studentProvisioning = studentProvisioning;
        _classes = classes;
        _transactions = transactions;
        _invitations = invitations;
        _text = text;
    }

    public async Task OnActionExecutionAsync(
        ActionExecutingContext context,
        ActionExecutionDelegate next)
    {
        if (!IsDirectStudentCreate(context, out var model))
        {
            await next();
            return;
        }

        if (context.Controller is not Controller controller ||
            !controller.User.IsInRole(RoleNames.SubjectSupervisor) ||
            !Guid.TryParse(
                controller.User.FindFirstValue(
                    ClaimTypes.NameIdentifier),
                out var actorUserId))
        {
            context.Result = new ForbidResult();
            return;
        }

        model.RoleOptions =
        [
            new(RoleNames.Teacher, "RoleTeacher"),
            new(RoleNames.Student, "RoleStudent")
        ];

        model.StudentClasses =
            await _classes.ListAsync(
                actorUserId,
                model.SchoolId,
                context.HttpContext.RequestAborted)
            ?? [];

        ValidateStudentFields(context, model);

        if (!context.ModelState.IsValid)
        {
            context.Result = controller.View("Create", model);
            return;
        }

        if (model.StudentClasses.All(
                x => x.Id != model.ClassGroupId!.Value))
        {
            context.ModelState.AddModelError(
                nameof(model.ClassGroupId),
                _text["StudentSetupClassNotFound"].Value);

            context.Result = controller.View("Create", model);
            return;
        }

        var management =
            await _users.GetManagementContextAsync(
                actorUserId,
                model.SchoolId,
                context.HttpContext.RequestAborted);

        if (management.Value is null ||
            management.Value.SchoolId != model.SchoolId)
        {
            context.Result = new ForbidResult();
            return;
        }

        Guid createdUserId;
        string passwordSetupToken;
        string schoolCulture;

        await using (
            var transaction =
                await _transactions.BeginAsync(
                    context.HttpContext.RequestAborted))
        {
            var create =
                await _users.CreateAsync(
                    actorUserId,
                    model.SchoolId,
                    new CreateSchoolUserRequest(
                        model.Email,
                        RoleNames.Teacher),
                    context.HttpContext.RequestAborted);

            if (!create.Succeeded ||
                !create.UserId.HasValue)
            {
                await transaction.RollbackAsync(
                    context.HttpContext.RequestAborted);

                if (TrySecurityFailure(
                        create.Errors,
                        out var failure))
                {
                    context.Result = failure;
                    return;
                }

                AddSchoolUserErrors(
                    context,
                    create.Errors);

                context.Result = controller.View("Create", model);
                return;
            }

            createdUserId = create.UserId.Value;

            var setup =
                await _studentProvisioning
                    .ConvertToStudentAsync(
                        actorUserId,
                        model.SchoolId,
                        createdUserId,
                        new StudentRoleProvisioningRequest(
                            model.StudentNumber,
                            model.FirstName,
                            model.LastName,
                            model.ClassGroupId!.Value),
                        context.HttpContext.RequestAborted);

            if (!setup.Succeeded)
            {
                await transaction.RollbackAsync(
                    context.HttpContext.RequestAborted);

                context.ModelState.AddModelError(
                    string.Empty,
                    _text[StudentSetupErrorKey(setup)].Value);

                context.Result = controller.View("Create", model);
                return;
            }

            // Role changes update the Identity security stamp, so the
            // token created with the temporary Teacher account must not
            // be used. Generate a fresh token only after Student setup.
            var password =
                await _users.GeneratePasswordSetupAsync(
                    actorUserId,
                    model.SchoolId,
                    createdUserId,
                    context.HttpContext.RequestAborted);

            if (!password.Succeeded ||
                string.IsNullOrWhiteSpace(
                    password.PasswordSetupToken))
            {
                await transaction.RollbackAsync(
                    context.HttpContext.RequestAborted);

                if (TrySecurityFailure(
                        password.Errors,
                        out var failure))
                {
                    context.Result = failure;
                    return;
                }

                AddSchoolUserErrors(
                    context,
                    password.Errors);

                context.Result = controller.View("Create", model);
                return;
            }

            passwordSetupToken =
                password.PasswordSetupToken;

            schoolCulture =
                string.IsNullOrWhiteSpace(password.SchoolCulture)
                    ? "en"
                    : password.SchoolCulture;

            await transaction.CommitAsync(
                context.HttpContext.RequestAborted);
        }

        var invitationCulture =
            GetInvitationCulture(schoolCulture);

        var link =
            controller.Url.Action(
                "SetPassword",
                "Account",
                new
                {
                    userId = createdUserId,
                    token = passwordSetupToken,
                    culture = invitationCulture
                },
                controller.Request.Scheme);

        UserInvitationDeliveryResult? delivery = null;

        if (!string.IsNullOrWhiteSpace(link))
        {
            delivery =
                await _invitations.SendAsync(
                    new UserInvitationDeliveryRequest(
                        model.Email.Trim(),
                        management.Value.SchoolName,
                        invitationCulture,
                        link,
                        "initial"),
                    context.HttpContext.RequestAborted);
        }

        if (delivery?.Succeeded == true)
        {
            controller.TempData["SchoolUserSuccess"] =
                _text[
                    "CreateUserInvitationSentSuccess",
                    model.Email.Trim()
                ].Value;
        }
        else
        {
            controller.TempData["SchoolUserSuccess"] =
                _text["CreateUserSuccess"].Value;

            controller.TempData["SchoolUserError"] =
                _text["InvitationDeliveryFailed"].Value;
        }

        context.Result =
            new RedirectToActionResult(
                "Details",
                "SchoolUsers",
                new
                {
                    id = createdUserId,
                    schoolId = model.SchoolId
                });
    }

    private static bool IsDirectStudentCreate(
        ActionExecutingContext context,
        out SchoolUserCreateViewModel model)
    {
        model = null!;

        return string.Equals(
                   context.ActionDescriptor.ControllerName,
                   "SchoolUsers",
                   StringComparison.Ordinal) &&
               string.Equals(
                   context.ActionDescriptor.ActionName,
                   "Create",
                   StringComparison.Ordinal) &&
               context.HttpContext.Request.Method == "POST" &&
               context.ActionArguments.TryGetValue(
                   "model",
                   out var value) &&
               value is SchoolUserCreateViewModel candidate &&
               string.Equals(
                   candidate.Role,
                   RoleNames.Student,
                   StringComparison.Ordinal) &&
               (model = candidate) is not null;
    }

    private void ValidateStudentFields(
        ActionExecutingContext context,
        SchoolUserCreateViewModel model)
    {
        if (string.IsNullOrWhiteSpace(model.StudentNumber))
        {
            AddRequired(
                context,
                nameof(model.StudentNumber));
        }

        if (string.IsNullOrWhiteSpace(model.FirstName))
        {
            AddRequired(
                context,
                nameof(model.FirstName));
        }

        if (string.IsNullOrWhiteSpace(model.LastName))
        {
            AddRequired(
                context,
                nameof(model.LastName));
        }

        if (!model.ClassGroupId.HasValue ||
            model.ClassGroupId.Value == Guid.Empty)
        {
            AddRequired(
                context,
                nameof(model.ClassGroupId));
        }
    }

    private void AddRequired(
        ActionExecutingContext context,
        string field) =>
        context.ModelState.AddModelError(
            field,
            _text["StudentSetupMissingFields"].Value);

    private void AddSchoolUserErrors(
        ActionExecutingContext context,
        IReadOnlyList<SchoolUserError> errors)
    {
        foreach (var error in errors)
        {
            context.ModelState.AddModelError(
                error.Field,
                _text[error.Code.ToString()].Value);
        }
    }

    private static bool TrySecurityFailure(
        IReadOnlyList<SchoolUserError> errors,
        out IActionResult result)
    {
        var error = errors.FirstOrDefault()?.Code;

        if (error == SchoolUserErrorCode.UserAccessDenied)
        {
            result = new ForbidResult();
            return true;
        }

        if (error == SchoolUserErrorCode.SchoolNotFound ||
            error == SchoolUserErrorCode.UserNotFound)
        {
            result = new NotFoundResult();
            return true;
        }

        result = null!;
        return false;
    }

    private static string GetInvitationCulture(
        string schoolCulture)
    {
        var current =
            CultureInfo.CurrentUICulture
                .TwoLetterISOLanguageName;

        if (string.Equals(
                current,
                "pl",
                StringComparison.OrdinalIgnoreCase))
        {
            return "pl";
        }

        return string.Equals(
                schoolCulture,
                "pl",
                StringComparison.OrdinalIgnoreCase)
            ? "pl"
            : "en";
    }

    private static string StudentSetupErrorKey(
        StudentRoleProvisioningResult setup)
    {
        if (setup.Error ==
            StudentRoleProvisioningErrorCode
                .UnderlyingOperationFailed)
        {
            return setup.AcademicError switch
            {
                Edulytics.Services.Academics.AcademicStructureErrorCode
                    .DuplicateStudentNumber =>
                    "StudentSetupDuplicateStudentNumber",

                Edulytics.Services.Academics.AcademicStructureErrorCode
                    .StudentSeatLimitReached =>
                    "StudentSetupSeatLimitReached",

                Edulytics.Services.Academics.AcademicStructureErrorCode
                    .ClassGroupNotFound =>
                    "StudentSetupClassNotFound",

                Edulytics.Services.Academics.AcademicStructureErrorCode
                    .DuplicateEnrollment =>
                    "StudentSetupDuplicateEnrollment",

                Edulytics.Services.Academics.AcademicStructureErrorCode
                    .InvalidCode =>
                    "StudentSetupInvalidStudentNumber",

                Edulytics.Services.Academics.AcademicStructureErrorCode
                    .InvalidName =>
                    "StudentSetupInvalidName",

                Edulytics.Services.Academics.AcademicStructureErrorCode
                    .Required =>
                    "StudentSetupMissingFields",

                _ =>
                    "StudentSetupFailed"
            };
        }

        return setup.Error switch
        {
            StudentRoleProvisioningErrorCode.MissingStudentNumber or
            StudentRoleProvisioningErrorCode.MissingFirstName or
            StudentRoleProvisioningErrorCode.MissingLastName or
            StudentRoleProvisioningErrorCode.MissingClass =>
                "StudentSetupMissingFields",

            StudentRoleProvisioningErrorCode.ClassNotFound =>
                "StudentSetupClassNotFound",

            StudentRoleProvisioningErrorCode.EnrollmentConflict =>
                "StudentSetupEnrollmentConflict",

            StudentRoleProvisioningErrorCode.RecoveryFailed =>
                "StudentSetupRecoveryFailed",

            _ =>
                "StudentSetupFailed"
        };
    }
}
