using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using Edulytics.Core.Enums;
using Edulytics.Services.Academics;
using Edulytics.Services.Curriculum;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Edulytics.Web.TagHelpers;

[HtmlTargetElement("form", Attributes = "asp-action")]
public sealed class AcademicStructureNormalUiTagHelper : TagHelper
{
    private readonly IExplicitCurriculumLevelUiQuery _explicitClasses;

    public AcademicStructureNormalUiTagHelper(
        IExplicitCurriculumLevelUiQuery explicitClasses)
    {
        _explicitClasses = explicitClasses;
    }

    [HtmlAttributeName("asp-action")]
    public string? Action { get; set; }

    [ViewContext]
    [HtmlAttributeNotBound]
    public ViewContext ViewContext { get; set; } = null!;

    public override async Task ProcessAsync(
        TagHelperContext context,
        TagHelperOutput output)
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

        output.Attributes.RemoveAll("asp-action");
        output.Attributes.SetAttribute(
            "action",
            "/school/academic-structure/student-placements/bulk");
        output.Attributes.SetAttribute("method", "post");
        output.Attributes.SetAttribute("class", "academic-student-placement-form");

        var actorId = Guid.TryParse(
            ViewContext.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier),
            out var parsedActorId)
            ? parsedActorId
            : Guid.Empty;

        var explicitClassItems = actorId == Guid.Empty
            ? Array.Empty<ExplicitCurriculumClassItem>()
            : (await _explicitClasses.ListClassesAsync(
                actorId,
                ViewContext.HttpContext.RequestAborted)).ToArray();
        var explicitByClassId = explicitClassItems
            .ToDictionary(x => x.ClassGroupId);

        var activeStudents = dashboard.StudentProfiles
            .Where(x => !x.IsArchived && x.Status == AcademicStructureStatus.Active)
            .OrderBy(x => x.DisplayName)
            .ThenBy(x => x.StudentNumber)
            .ToArray();

        var html = new StringBuilder();
        html.Append("<style>")
            .Append(".academic-student-placement-form{display:grid;gap:14px}")
            .Append(".student-picker-toolbar{display:flex;gap:10px;align-items:center;flex-wrap:wrap}")
            .Append(".student-picker-search{flex:1 1 260px;min-width:0}")
            .Append(".student-picker-summary{font-size:.9rem;color:#64748b;margin-left:auto}")
            .Append(".academic-bulk-students{border:1px solid #d8e1ee;border-radius:12px;padding:6px;max-height:360px;overflow:auto;background:#fff}")
            .Append(".academic-bulk-student{display:flex;align-items:center;gap:10px;padding:9px 10px;border-radius:9px;cursor:pointer;margin:0}")
            .Append(".academic-bulk-student:hover{background:#f5f8ff}")
            .Append(".academic-bulk-student input{width:18px;height:18px;flex:0 0 18px;margin:0}")
            .Append(".academic-bulk-student-main{display:flex;align-items:baseline;gap:7px;min-width:0}")
            .Append(".academic-bulk-student-name{font-weight:600;overflow:hidden;text-overflow:ellipsis;white-space:nowrap}")
            .Append(".academic-bulk-student-number{color:#64748b;font-size:.86rem;white-space:nowrap}")
            .Append(".student-picker-empty{padding:18px;text-align:center;color:#64748b;display:none}")
            .Append(".student-picker-actions{display:flex;gap:8px;flex-wrap:wrap}")
            .Append("@media(max-width:640px){.student-picker-summary{width:100%;margin-left:0}.academic-bulk-students{max-height:300px}}")
            .Append("</style>");

        html.Append("<h3>Enroll or move students</h3>");
        html.Append("<p class=\"academic-help\">Search and select students, then choose the target Class. Existing students can move only between classes in the same Grade.</p>");
        html.Append("<label for=\"bulk-student-class\">Class</label>");
        html.Append("<select id=\"bulk-student-class\" name=\"classGroupId\" required>");
        html.Append("<option value=\"\">Select</option>");

        foreach (var classGroup in dashboard.ClassGroups
                     .Where(x => x.Status == AcademicStructureStatus.Active)
                     .OrderBy(x => x.GradeLevelName)
                     .ThenBy(x => x.Name))
        {
            var classLabel = explicitByClassId.TryGetValue(
                classGroup.Id,
                out var explicitClass)
                ? explicitClass.DisplayLabel
                : string.Join(
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
        html.Append("<div class=\"student-picker-toolbar\">")
            .Append("<input id=\"student-picker-search\" class=\"student-picker-search\" type=\"search\" placeholder=\"Search by student name or number\" autocomplete=\"off\" />")
            .Append("<span id=\"student-picker-summary\" class=\"student-picker-summary\">0 selected · ")
            .Append(activeStudents.Length)
            .Append(" students</span></div>");
        html.Append("<div class=\"student-picker-actions\">")
            .Append("<button type=\"button\" class=\"school-button\" id=\"student-picker-select-visible\">Select visible</button>")
            .Append("<button type=\"button\" class=\"school-button\" id=\"student-picker-clear\">Clear selection</button>")
            .Append("</div>");
        html.Append("<fieldset class=\"academic-bulk-students\" id=\"academic-bulk-students\"><legend class=\"visually-hidden\">Students</legend>");

        foreach (var student in activeStudents)
        {
            var searchable = $"{student.DisplayName} {student.StudentNumber}".ToLowerInvariant();
            html.Append("<label class=\"academic-bulk-student\" data-student-search=\"")
                .Append(HtmlEncoder.Default.Encode(searchable))
                .Append("\">")
                .Append("<input type=\"checkbox\" name=\"studentProfileIds\" value=\"")
                .Append(student.Id.ToString("D"))
                .Append("\" />")
                .Append("<span class=\"academic-bulk-student-main\"><span class=\"academic-bulk-student-name\">")
                .Append(HtmlEncoder.Default.Encode(student.DisplayName))
                .Append("</span>");

            if (!string.IsNullOrWhiteSpace(student.StudentNumber))
            {
                html.Append("<span class=\"academic-bulk-student-number\">#")
                    .Append(HtmlEncoder.Default.Encode(student.StudentNumber))
                    .Append("</span>");
            }

            html.Append("</span></label>");
        }

        html.Append("<div class=\"student-picker-empty\" id=\"student-picker-empty\">No students match this search.</div>")
            .Append("</fieldset>");
        html.Append("<button class=\"school-button school-button-primary\" type=\"submit\">Enroll / move selected students</button>");
        html.Append("<script>(function(){")
            .Append("const root=document.getElementById('academic-bulk-students');if(!root)return;")
            .Append("const search=document.getElementById('student-picker-search');const summary=document.getElementById('student-picker-summary');")
            .Append("const rows=[...root.querySelectorAll('.academic-bulk-student')];const boxes=rows.map(r=>r.querySelector('input[type=checkbox]'));")
            .Append("const empty=document.getElementById('student-picker-empty');")
            .Append("function visibleRows(){return rows.filter(r=>r.style.display!=='none');}")
            .Append("function updateSummary(){const selected=boxes.filter(b=>b.checked).length;summary.textContent=selected+' selected · "+activeStudents.Length+" students';}")
            .Append("function filter(){const q=(search.value||'').trim().toLowerCase();let shown=0;rows.forEach(r=>{const ok=!q||(r.dataset.studentSearch||'').includes(q);r.style.display=ok?'flex':'none';if(ok)shown++;});empty.style.display=shown?'none':'block';}")
            .Append("search.addEventListener('input',filter);boxes.forEach(b=>b.addEventListener('change',updateSummary));")
            .Append("document.getElementById('student-picker-select-visible').addEventListener('click',()=>{visibleRows().forEach(r=>r.querySelector('input[type=checkbox]').checked=true);updateSummary();});")
            .Append("document.getElementById('student-picker-clear').addEventListener('click',()=>{boxes.forEach(b=>b.checked=false);updateSummary();});")
            .Append("updateSummary();})();</script>");

        output.Content.SetHtmlContent(html.ToString());
    }
}
