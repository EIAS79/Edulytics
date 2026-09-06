using System.Security.Claims;
using Edulytics.Services.Assessments;
using Edulytics.Web.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Edulytics.Web.Filters;

public sealed class AssessmentMaxScoreLockFilter : IAsyncActionFilter
{
    private readonly IAssessmentService _assessments;

    public AssessmentMaxScoreLockFilter(IAssessmentService assessments)
    {
        _assessments = assessments;
    }

    public async Task OnActionExecutionAsync(
        ActionExecutingContext context,
        ActionExecutionDelegate next)
    {
        var hasActionName = context.ActionDescriptor.RouteValues.TryGetValue(
            "action",
            out var actionName);

        if (context.Controller is not AssessmentsController ||
            !HttpMethods.IsPost(context.HttpContext.Request.Method) ||
            !hasActionName ||
            !string.Equals(actionName, "Edit", StringComparison.Ordinal) ||
            !context.ActionArguments.TryGetValue("id", out var idValue) ||
            idValue is not Guid assessmentId ||
            !context.ActionArguments.ContainsKey("maxScore"))
        {
            await next();
            return;
        }

        if (!Guid.TryParse(
                context.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier),
                out var actorUserId))
        {
            await next();
            return;
        }

        var details = await _assessments.GetDetailsAsync(
            actorUserId,
            assessmentId,
            context.HttpContext.RequestAborted);

        if (details.Value is not null && details.Value.Questions.Count > 0)
        {
            // Once questions exist, Maximum score is owned by the assessment
            // question structure. Ignore any forged/manual POST value.
            context.ActionArguments["maxScore"] =
                details.Value.Assessment.MaxScore;
        }

        await next();
    }
}
