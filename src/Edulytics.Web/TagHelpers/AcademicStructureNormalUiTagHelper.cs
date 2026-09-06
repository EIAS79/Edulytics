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
            output.SuppressOutput();
    }
}
