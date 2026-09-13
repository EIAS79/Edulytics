using System.Security.Claims;
using Edulytics.Core.Constants;
using Edulytics.Core.Enums;
using Edulytics.Core.Interfaces;
using Edulytics.Services.Imports;
using Edulytics.Web.Email;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Edulytics.Web.Controllers;

[Authorize(Policy = "DataImport")]
public sealed class ImportBatchActionsController(
    IDataImportService imports,
    ImportBatchEditingService editing,
    ISchoolUserRepository schoolUsers,
    IUserInvitationDeliveryService invitations) : Controller
{
    [HttpPost("/school/imports/{batchId:guid}/remove-rows")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RemoveRows(
        Guid batchId,
        string? rowVersion,
        int[]? rowNumbers,
        CancellationToken cancellationToken)
    {
        if (!TryActor(out var actorId))
            return Forbid();
        if (!TryRowVersion(rowVersion, out var version))
            return BadRequest();

        var result = await editing.RemoveRowsAsync(
            actorId,
            batchId,
            version,
            rowNumbers ?? [],
            cancellationToken);

        TempData[result.Succeeded ? "ImportSuccess" : "ImportError"] = result.Succeeded
            ? "Selected rows were removed. Review the remaining validation results before confirming."
            : "The selected rows could not be removed. Reload the batch and try again.";

        return RedirectToAction("Details", "Imports", new { batchId });
    }

    [HttpPost("/school/imports/{batchId:guid}/discard")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Discard(
        Guid batchId,
        CancellationToken cancellationToken)
    {
        if (!TryActor(out var actorId))
            return Forbid();

        var result = await editing.DiscardBatchAsync(
            actorId,
            batchId,
            cancellationToken);

        TempData[result.Succeeded ? "ImportSuccess" : "ImportError"] = result.Succeeded
            ? "The unconfirmed import batch was discarded."
            : "This import batch could not be discarded. Completed imports are retained as history.";

        return RedirectToAction("Index", "Imports");
    }

    [HttpPost("/school/imports/{batchId:guid}/resend-invitation")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ResendInvitation(
        Guid batchId,
        int rowNumber,
        CancellationToken cancellationToken)
    {
        if (!TryActor(out var actorId))
            return Forbid();

        var batchResult = await imports.GetBatchAsync(actorId, batchId, cancellationToken);
        if (!batchResult.Succeeded || batchResult.Value is null)
            return Forbid();

        var batch = batchResult.Value;
        if (batch.Status != ImportBatchStatus.Completed ||
            batch.Type is not (ImportType.Students or ImportType.Teachers or ImportType.SubjectSupervisors))
            return BadRequest();

        var row = batch.PreviewRows.FirstOrDefault(x => x.RowNumber == rowNumber);
        if (row is null || !TryValue(row.Values, "Email", out var email))
            return BadRequest();

        var actor = await schoolUsers.GetActorAsync(actorId, cancellationToken);
        if (actor?.SchoolId is not Guid schoolId)
            return Forbid();

        var users = await schoolUsers.ListBySchoolAsync(schoolId, cancellationToken);
        var user = users.FirstOrDefault(x => string.Equals(x.Email, email, StringComparison.OrdinalIgnoreCase));
        if (user is null)
            return BadRequest();

        var setup = await schoolUsers.GeneratePasswordSetupAsync(schoolId, user.Id, cancellationToken);
        if (!setup.Succeeded || string.IsNullOrWhiteSpace(setup.PasswordSetupToken))
        {
            TempData["ImportError"] = "A new invitation link could not be generated.";
            return RedirectToAction("Details", "Imports", new { batchId });
        }

        var culture = System.Globalization.CultureInfo.CurrentUICulture.TwoLetterISOLanguageName == "pl" ? "pl" : "en";
        var link = Url.Action("SetPassword", "Account", new { userId = user.Id, token = setup.PasswordSetupToken, culture }, Request.Scheme);
        if (string.IsNullOrWhiteSpace(link))
            return BadRequest();

        var delivery = await invitations.SendAsync(
            new UserInvitationDeliveryRequest(user.Email, "Edulytics", culture, link, "bulk-import-resend"),
            cancellationToken);

        await editing.RecordInvitationOutcomesAsync(
            actorId,
            batchId,
            [new ImportInvitationOutcome(user.Email, delivery.Succeeded)],
            cancellationToken);

        TempData[delivery.Succeeded ? "ImportSuccess" : "ImportError"] = delivery.Succeeded
            ? "Invitation email sent successfully."
            : "The account is imported, but the invitation email could not be delivered. You can try again.";

        return RedirectToAction("Details", "Imports", new { batchId });
    }

    private bool TryActor(out Guid actorId) =>
        Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out actorId);

    private static bool TryRowVersion(string? value, out byte[] bytes)
    {
        bytes = [];
        try
        {
            if (string.IsNullOrWhiteSpace(value)) return false;
            bytes = Convert.FromBase64String(value);
            return bytes.Length > 0;
        }
        catch (FormatException) { return false; }
    }

    private static bool TryValue(IReadOnlyDictionary<string, string> values, string key, out string value)
    {
        value = values.FirstOrDefault(x => string.Equals(x.Key, key, StringComparison.OrdinalIgnoreCase)).Value ?? string.Empty;
        return value.Length > 0;
    }
}
