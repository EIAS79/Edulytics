using System.Text;
using System.Text.Encodings.Web;
using Edulytics.Core.Enums;
using Edulytics.Services.Academics;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Edulytics.Web.TagHelpers;

[HtmlTargetElement("form", Attributes = "asp-action")]
public sealed class AcademicStructureNormalUiTagHelper : TagHelper
{
    [HtmlAttributeName("asp-action")]
    public string? Action { get; set; }

    [ViewContext]
    [HtmlAttributeNotBound]
    public ViewContext ViewContext { get; set; } = null!;

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        var controller = ViewContext.RouteData.Values["controller"]?.ToString();
        if (!string.Equals(controller, "AcademicStructure", StringComparison.OrdinalIgnoreCase))
            return;

        if (string.Equals(Action, "CreateStudentProfile", StringComparison.Ordinal))
        {
            output.SuppressOutput();
            return;
        }

        if (!string.Equals(Action, "CreateStudentEnrollment", StringComparison.Ordinal))
            return;

        if (ViewContext.ViewData.Model is not AcademicStructureDashboard dashboard)
            return;

        output.Attributes.SetAttribute(
            "action",
            "/school/academic-structure/student-placements/bulk");
        output.Attributes.SetAttribute("method", "post");

        var html = new StringBuilder();
        html.Append("<h3>Enroll or move students</h3>");
        html.Append("<p class=\"academic-help\">Select one or more students. Existing students can move only between classes in the same Grade.</p>");
        html.Append("<label for=\"bulk-student-class\">Class</label>");
        html.Append("<select id=\"bulk-student-class\" name=\"classGroupId\" required>");
        html.Append("<option value=\"\">Select</option>");

        foreach (var classGroup in dashboard.ClassGroups
                     .Where(x => x.Status == AcademicStructureStatus.Active)
                     .OrderBy(x => x.GradeLevelName)
                     .ThenBy(x => x.Name))
        {
            var classLabel = string.Join(
                " · ",
                new[]
                {
                    classGroup.Name,
                    classGroup.GradeLevelName,
                    classGroup.AcademicProgramName
                }.Where(x => !string.IsNullOrWhiteSpace(x)));

            html.Append("<option value=\"")
                .Append(classGroup.Id.ToString("D"))
                .Append("\">")
                .Append(HtmlEncoder.Default.Encode(classLabel))
                .Append("</option>");
        }

        html.Append("</select>");
        html.Append("<fieldset class=\"academic-bulk-students\"><legend>Students</legend>");

        foreach (var student in dashboard.StudentProfiles
                     .Where(x => !x.IsArchived && x.Status == AcademicStructureStatus.Active)
                     .OrderBy(x => x.DisplayName))
        {
            html.Append("<label class=\"academic-bulk-student\">")
                .Append("<input type=\"checkbox\" name=\"studentProfileIds\" value=\"")
                .Append(student.Id.ToString("D"))
                .Append("\" /> ")
                .Append(HtmlEncoder.Default.Encode(student.DisplayName));

            if (!string.IsNullOrWhiteSpace(student.StudentNumber))
            {
                html.Append(" <small>(")
                    .Append(HtmlEncoder.Default.Encode(student.StudentNumber))
                    .Append(")</small>");
            }

            html.Append("</label>");
        }

        html.Append("</fieldset>");
        html.Append("<button class=\"school-button school-button-primary\" type=\"submit\">Enroll / move selected students</button>");

        output.Content.SetHtmlContent(html.ToString());
    }
}
