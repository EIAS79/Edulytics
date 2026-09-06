using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Edulytics.Web.TagHelpers;

[HtmlTargetElement("form", Attributes = "asp-action")]
public sealed class AssessmentBuilderApprovalFormTagHelper : TagHelper
{
    public override int Order => 10_000;

    [HtmlAttributeName("asp-action")]
    public string? Action { get; set; }

    [ViewContext]
    [HtmlAttributeNotBound]
    public ViewContext ViewContext { get; set; } = null!;

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        var controller = ViewContext.RouteData.Values["controller"]?.ToString();
        if (!string.Equals(controller, "AssessmentBuilder", StringComparison.OrdinalIgnoreCase))
            return;

        if (!Guid.TryParse(
                ViewContext.RouteData.Values["assessmentId"]?.ToString(),
                out var assessmentId))
            return;

        string? path = null;
        if (string.Equals(Action, "ApproveAll", StringComparison.Ordinal))
        {
            path = $"/school/assessments/{assessmentId:D}/builder/approval/drafts";
        }
        else if (string.Equals(Action, "Approve", StringComparison.Ordinal))
        {
            var rawQuestionId = context.AllAttributes["asp-route-questionId"]?.Value?.ToString();
            if (Guid.TryParse(rawQuestionId, out var questionId))
                path = $"/school/assessments/{assessmentId:D}/builder/approval/question/{questionId:D}";
        }

        if (path is null)
            return;

        output.Attributes.RemoveAll("asp-action");
        output.Attributes.RemoveAll("asp-route-assessmentId");
        output.Attributes.RemoveAll("asp-route-questionId");
        output.Attributes.SetAttribute("action", path);
        output.Attributes.SetAttribute("method", "post");
    }
}
