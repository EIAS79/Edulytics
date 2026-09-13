using System.Security.Claims;
using System.Text;
using Edulytics.Core.Constants;
using Edulytics.Core.Enums;
using Edulytics.Services.Assessments;
using Edulytics.Services.Imports;
using Edulytics.Web.Email;
using Edulytics.Web.Imports;
using Edulytics.Web.ViewModels.Imports;
using Edulytics.Web.Resilience;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Timeouts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Localization;

namespace Edulytics.Web.Controllers;

[Authorize(Policy = "DataImport")]
public sealed class ImportsController : Controller
{
    private const string ImportManagerRoles =
        RoleNames.SubjectSupervisor + "," +
        RoleNames.Teacher;

    private readonly IDataImportService _imports;
    private readonly IAssessmentService _assessments;
    private readonly IUserInvitationDeliveryService _invitations;
    private readonly IStringLocalizer<ImportResource> _text;

    public ImportsController(
        IDataImportService imports,
        IAssessmentService assessments,
        IUserInvitationDeliveryService invitations,
        IStringLocalizer<ImportResource> text)
    {
        _imports = imports;
        _assessments = assessments;
        _invitations = invitations;
        _text = text;
    }

    [HttpGet("/school/imports")]
    public async Task<IActionResult> Index(
        CancellationToken cancellationToken)
    {
        if (!TryActor(out var actorId))
            return Forbid();

        var result = await _imports.GetWorkspaceAsync(
            actorId,
            cancellationToken);

        if (!result.Succeeded)
            return Failure(result.Error);

        var workspace = result.Value!;
        var productWorkspace = new ImportWorkspace(
            MathOnlyImportAdapter.FilterOptions(workspace.AllowedTypes),
            workspace.Batches);

        return View(new ImportIndexViewModel(productWorkspace));
    }

    [HttpGet("/school/imports/{batchId:guid}")]
    public async Task<IActionResult> Details(
        Guid batchId,
        CancellationToken cancellationToken)
    {
        if (!TryActor(out var actorId))
            return Forbid();

        var result = await _imports.GetBatchAsync(
            actorId,
            batchId,
            cancellationToken);

        if (!result.Succeeded)
            return Failure(result.Error);

        return View(new ImportDetailsViewModel(result.Value!));
    }

    [Authorize(Roles = ImportManagerRoles)]
    [HttpPost("/school/imports/upload")]
    [ValidateAntiForgeryToken]
    [RequestSizeLimit(ImportFileParser.MaxBytes + 65536)]
    [RequestTimeout(BackendResiliencePolicyNames.Import)]
    [EnableRateLimiting(BackendResiliencePolicyNames.ImportConcurrency)]
    public async Task<IActionResult> Upload(
        ImportType importType,
        IFormFile? file,
        CancellationToken cancellationToken)
    {
        if (!TryActor(out var actorId))
            return Forbid();

        if (!Enum.IsDefined(importType) ||
            !MathOnlyImportAdapter.IsSupported(importType))
        {
            return BadRequest();
        }

        if (file is null || file.Length <= 0)
        {
            TempData["ImportError"] = _text["ErrorEmptyFile"].Value;
            return RedirectToAction(nameof(Index));
        }

        if (file.Length > ImportFileParser.MaxBytes)
        {
            TempData["ImportError"] = _text["ErrorFileTooLarge"].Value;
            return RedirectToAction(nameof(Index));
        }

        await using var stream = new MemoryStream();
        await file.CopyToAsync(stream, cancellationToken);
        var rawBytes = stream.ToArray();

        AdaptedImportUpload upload;
        if (importType == ImportType.AssessmentResults)
        {
            var assessmentWorkspace = await _assessments.GetWorkspaceAsync(
                actorId,
                cancellationToken);

            if (assessmentWorkspace.Value is null)
                return Forbid();

            upload = MathOnlyImportAdapter.NormalizeAssessmentResults(
                file.FileName,
                rawBytes,
                assessmentWorkspace.Value);
        }
        else
        {
            upload = MathOnlyImportAdapter.NormalizeUpload(
                importType,
                file.FileName,
                rawBytes);
        }

        var result = await _imports.UploadAsync(
            actorId,
            importType,
            upload.FileName,
            upload.Bytes,
            cancellationToken);

        if (!result.Succeeded)
        {
            if (result.Error == ImportErrorCode.AccessDenied)
                return Forbid();

            TempData["ImportError"] =
                _text[ErrorResourceKey(result.Error)].Value;

            return RedirectToAction(nameof(Index));
        }

        TempData["ImportSuccess"] = _text["SuccessUploaded"].Value;
        return RedirectToAction(
            nameof(Details),
            new { batchId = result.Value!.Id });
    }

    [Authorize(Roles = ImportManagerRoles)]
    [HttpPost("/school/imports/{batchId:guid}/confirm")]
    [ValidateAntiForgeryToken]
    [RequestTimeout(BackendResiliencePolicyNames.Import)]
    [EnableRateLimiting(BackendResiliencePolicyNames.ImportConcurrency)]
    public async Task<IActionResult> Confirm(
        Guid batchId,
        string? rowVersion,
        CancellationToken cancellationToken)
    {
        if (!TryActor(out var actorId))
            return Forbid();

        var batch = await _imports.GetBatchAsync(
            actorId,
            batchId,
            cancellationToken);

        if (!batch.Succeeded)
            return Failure(batch.Error);

        if (!MathOnlyImportAdapter.IsSupported(batch.Value!.Type))
            return BadRequest();

        if (!TryRowVersion(rowVersion, out var bytes))
        {
            TempData["ImportError"] =
                _text["ErrorConcurrencyConflict"].Value;

            return RedirectToAction(
                nameof(Details),
                new { batchId });
        }

        var result = await _imports.ConfirmAsync(
            actorId,
            batchId,
            bytes,
            cancellationToken);

        if (!result.Succeeded)
        {
            if (result.Error == ImportErrorCode.AccessDenied)
                return Forbid();

            TempData["ImportError"] =
                _text[ErrorResourceKey(result.Error)].Value;

            return RedirectToAction(
                nameof(Details),
                new { batchId });
        }

        var invitationResult = await DeliverInvitationsAsync(
            result.Value!.Invitations,
            cancellationToken);

        TempData["ImportSuccess"] = invitationResult switch
        {
            (0, 0) => _text["SuccessCompleted"].Value,
            (_, 0) =>
                $"{_text["SuccessCompleted"].Value} {invitationResult.Sent} account invitation(s) sent.",
            _ =>
                $"{_text["SuccessCompleted"].Value} {invitationResult.Sent} invitation(s) sent; {invitationResult.Failed} could not be delivered and can be resent from School users."
        };

        return RedirectToAction(
            nameof(Details),
            new { batchId = result.Value.Id });
    }

    [HttpGet("/school/imports/template/{importType}")]
    public async Task<IActionResult> Template(
        ImportType importType,
        CancellationToken cancellationToken)
    {
        if (!TryActor(out var actorId))
            return Forbid();

        if (!Enum.IsDefined(importType) ||
            !MathOnlyImportAdapter.IsSupported(importType))
        {
            return NotFound();
        }

        var workspace = await _imports.GetWorkspaceAsync(
            actorId,
            cancellationToken);

        if (!workspace.Succeeded ||
            !workspace.Value!.AllowedTypes.Any(x => x.Type == importType))
        {
            return Forbid();
        }

        var headers = MathOnlyImportAdapter.TemplateHeaders(
            importType,
            _imports.GetTemplateHeaders(importType));

        var content = string.Join(",", headers) + Environment.NewLine;

        return File(
            Encoding.UTF8.GetBytes(content),
            "text/csv",
            $"edulytics-{importType}.csv");
    }

    private async Task<(int Sent, int Failed)> DeliverInvitationsAsync(
        IReadOnlyList<ImportInvitationCandidate> invitations,
        CancellationToken cancellationToken)
    {
        if (invitations.Count == 0)
            return (0, 0);

        var sent = 0;
        var failed = 0;
        var culture = GetInvitationCulture();

        foreach (var candidate in invitations)
        {
            var link = BuildPasswordSetupLink(
                candidate.UserId,
                candidate.PasswordSetupToken,
                culture);

            if (link is null)
            {
                failed++;
                continue;
            }

            var delivery = await _invitations.SendAsync(
                new UserInvitationDeliveryRequest(
                    candidate.Email,
                    candidate.SchoolName,
                    culture,
                    link,
                    "bulk-import"),
                cancellationToken);

            if (delivery.Succeeded)
                sent++;
            else
                failed++;
        }

        return (sent, failed);
    }

    private static string GetInvitationCulture()
    {
        var culture = System.Globalization.CultureInfo
            .CurrentUICulture
            .TwoLetterISOLanguageName;

        return string.Equals(
            culture,
            "pl",
            StringComparison.OrdinalIgnoreCase)
            ? "pl"
            : "en";
    }

    private string? BuildPasswordSetupLink(
        Guid userId,
        string? token,
        string culture)
    {
        if (userId == Guid.Empty || string.IsNullOrWhiteSpace(token))
            return null;

        return Url.Action(
            "SetPassword",
            "Account",
            new
            {
                userId,
                token,
                culture
            },
            Request.Scheme);
    }

    private bool TryActor(out Guid actorId) =>
        Guid.TryParse(
            User.FindFirstValue(ClaimTypes.NameIdentifier),
            out actorId);

    private IActionResult Failure(ImportErrorCode? error) =>
        error switch
        {
            ImportErrorCode.BatchNotFound => NotFound(),
            _ => Forbid()
        };

    private static bool TryRowVersion(
        string? value,
        out byte[] bytes)
    {
        bytes = [];
        if (string.IsNullOrWhiteSpace(value))
            return false;

        try
        {
            bytes = Convert.FromBase64String(value);
            return bytes.Length > 0;
        }
        catch (FormatException)
        {
            return false;
        }
    }

    private static string ErrorResourceKey(ImportErrorCode? error) =>
        error switch
        {
            ImportErrorCode.AccessDenied => "ErrorAccessDenied",
            ImportErrorCode.SchoolNotActive => "ErrorSchoolNotActive",
            ImportErrorCode.UnsupportedFile => "ErrorUnsupportedFile",
            ImportErrorCode.InvalidFile => "ErrorInvalidFile",
            ImportErrorCode.FileTooLarge => "ErrorFileTooLarge",
            ImportErrorCode.TooManyRows => "ErrorTooManyRows",
            ImportErrorCode.TooManyColumns => "ErrorTooManyColumns",
            ImportErrorCode.DuplicateHeader => "ErrorDuplicateHeader",
            ImportErrorCode.EmptyFile => "ErrorEmptyFile",
            ImportErrorCode.BatchNotFound => "ErrorBatchNotFound",
            ImportErrorCode.BatchHasErrors => "ErrorBatchHasErrors",
            ImportErrorCode.BatchStateChanged => "ErrorBatchStateChanged",
            ImportErrorCode.ConcurrencyConflict => "ErrorConcurrencyConflict",
            ImportErrorCode.SeatLimitReached => "ErrorSeatLimitReached",
            _ => "ErrorPersistence"
        };
}
