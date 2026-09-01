#!/usr/bin/env python3
from __future__ import annotations

import json
import re
from pathlib import Path

ROOT = Path(__file__).resolve().parents[2]


def read(path: str) -> str:
    return (ROOT / path).read_text(encoding="utf-8")


def write(path: str, text: str) -> None:
    p = ROOT / path
    p.parent.mkdir(parents=True, exist_ok=True)
    p.write_text(text, encoding="utf-8")


def replace_once(path: str, old: str, new: str) -> None:
    text = read(path)
    if old not in text:
        if new in text:
            print(f"PASS already patched: {path}")
            return
        raise SystemExit(f"FAIL expected patch anchor missing: {path}\n{old[:220]}")
    write(path, text.replace(old, new, 1))
    print(f"PASS patched: {path}")


def replace_all(path: str, old: str, new: str, minimum: int = 1) -> None:
    text = read(path)
    count = text.count(old)
    if count < minimum:
        if new in text:
            print(f"PASS already patched: {path}")
            return
        raise SystemExit(f"FAIL expected at least {minimum} replacements in {path}; got {count}")
    write(path, text.replace(old, new))
    print(f"PASS patched {count} occurrence(s): {path}")


# ---------------------------------------------------------------------------
# 1. Supporting is a lesson role, not a synonym for 'no formal mapping'.
# ---------------------------------------------------------------------------
replace_once(
    "src/Edulytics.Core/Curriculum/CanonicalLessonContentPack.cs",
    '''            if ((!lesson.IsSupporting && lesson.OutcomeCodes.Count == 0) ||
                (lesson.IsSupporting && lesson.OutcomeCodes.Count != 0) ||
                lesson.OutcomeCodes.Any(string.IsNullOrWhiteSpace))
            {
                throw new InvalidOperationException(
                    $"Lesson {lesson.LessonCode} must have exact OutcomeCodes when aligned and zero OutcomeCodes when Supporting.");
            }
''',
    '''            if ((lesson.IsSupporting && lesson.OutcomeCodes.Count != 0) ||
                lesson.OutcomeCodes.Any(string.IsNullOrWhiteSpace))
            {
                throw new InvalidOperationException(
                    $"Lesson {lesson.LessonCode} must have zero OutcomeCodes when Supporting; curriculum lessons may remain unmapped when no verified formal mapping exists.");
            }
''')

role_registry = r'''using System.Text.Json;
using System.Text.Json.Serialization;

namespace Edulytics.Core.Curriculum;

/// <summary>
/// Runtime lesson-role registry sourced from the reviewed embedded canonical
/// content packs. A lesson can be a primary curriculum lesson without claiming
/// a formal outcome mapping; Supporting is an explicit editorial role.
/// </summary>
public static class CanonicalLessonRoleRegistry
{
    private static readonly Lazy<IReadOnlyDictionary<string, bool>> Roles =
        new(Build, LazyThreadSafetyMode.ExecutionAndPublication);

    public static bool TryGetIsSupporting(
        string? lessonCode,
        out bool isSupporting)
    {
        if (string.IsNullOrWhiteSpace(lessonCode))
        {
            isSupporting = false;
            return false;
        }

        return Roles.Value.TryGetValue(
            lessonCode.Trim(),
            out isSupporting);
    }

    private static IReadOnlyDictionary<string, bool> Build()
    {
        var assembly = typeof(CanonicalLessonRoleRegistry).Assembly;
        var result = new Dictionary<string, bool>(StringComparer.Ordinal);
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
        options.Converters.Add(new JsonStringEnumConverter());

        foreach (var resourceName in assembly
            .GetManifestResourceNames()
            .Where(x => x.EndsWith(
                ".lesson-content-pack.json",
                StringComparison.Ordinal)))
        {
            using var stream = assembly.GetManifestResourceStream(resourceName)
                ?? throw new InvalidOperationException(
                    $"Embedded lesson content resource is missing: {resourceName}.");

            var document = JsonSerializer.Deserialize<CanonicalLessonContentPackDocument>(
                stream,
                options)
                ?? throw new InvalidOperationException(
                    $"Embedded lesson content resource is invalid: {resourceName}.");

            foreach (var lesson in document.Lessons)
            {
                if (result.TryGetValue(lesson.LessonCode, out var existing))
                {
                    // A later reviewed content version may supersede a pilot.
                    // Core curriculum wins over Supporting when both versions
                    // intentionally reference the same stable lesson identity.
                    result[lesson.LessonCode] = existing && lesson.IsSupporting;
                }
                else
                {
                    result.Add(lesson.LessonCode, lesson.IsSupporting);
                }
            }
        }

        return result;
    }
}
'''
write(
    "src/Edulytics.Core/Curriculum/CanonicalLessonRoleRegistry.cs",
    role_registry)

replace_once(
    "src/Edulytics.Core/Lessons/LessonContentPersistenceContracts.cs",
    '''    int LogicalLevelTo,
    int SortOrder,
    int OfficialOutcomeCount);
''',
    '''    int LogicalLevelTo,
    int SortOrder,
    int OfficialOutcomeCount)
{
    public bool? IsSupporting { get; init; }
}
''')

# Repository: enrich the existing record with the canonical editorial role.
repo_path = "src/Edulytics.Data/Repositories/LessonContentRepository.cs"
repo = read(repo_path)
anchor = '''                x.LogicalLevelTo,
                x.SortOrder,
                countByLesson.GetValueOrDefault(x.Id)))
            .ToArray();
'''
replacement = '''                x.LogicalLevelTo,
                x.SortOrder,
                countByLesson.GetValueOrDefault(x.Id))
            {
                IsSupporting =
                    CanonicalLessonRoleRegistry.TryGetIsSupporting(
                        x.Code,
                        out var isSupporting)
                        ? isSupporting
                        : null
            })
            .ToArray();
'''
if anchor not in repo:
    if replacement not in repo:
        raise SystemExit("FAIL LessonContentRepository role patch anchor missing")
else:
    if "using Edulytics.Core.Curriculum;" not in repo:
        repo = repo.replace(
            "using Edulytics.Core.Entities;\n",
            "using Edulytics.Core.Entities;\nusing Edulytics.Core.Curriculum;\n",
            1)
    repo = repo.replace(anchor, replacement, 1)
    write(repo_path, repo)

replace_once(
    "src/Edulytics.Services/LessonContent/LessonContentContracts.cs",
    '''public sealed record CanonicalLessonLibraryItem(
    Guid LessonId,string LessonCode,string LessonTitle,string UnitTitle,int SortOrder,
    CanonicalLessonContentStatus? Status,DateTime? PublishedAtUtc,bool HasOfficialAlignment);
''',
    '''public sealed record CanonicalLessonLibraryItem(
    Guid LessonId,string LessonCode,string LessonTitle,string UnitTitle,int SortOrder,
    CanonicalLessonContentStatus? Status,DateTime? PublishedAtUtc,bool HasOfficialAlignment)
{
    public bool IsSupporting { get; init; }
}
''')

service_path = "src/Edulytics.Services/LessonContent/LessonContentService.cs"
service = read(service_path)
old_item = '''                        return new CanonicalLessonLibraryItem(
                            lesson.Id,
                            lesson.Code,
                            lesson.Title,
                            lesson.UnitTitle,
                            lesson.SortOrder,
                            content?.Status,
                            content?.PublishedAtUtc,
                            LessonContentPolicy.IsStandaloneCanonicalTarget(
                                lesson.OfficialOutcomeCount));
'''
new_item = '''                        return new CanonicalLessonLibraryItem(
                            lesson.Id,
                            lesson.Code,
                            lesson.Title,
                            lesson.UnitTitle,
                            lesson.SortOrder,
                            content?.Status,
                            content?.PublishedAtUtc,
                            LessonContentPolicy.IsStandaloneCanonicalTarget(
                                lesson.OfficialOutcomeCount))
                        {
                            IsSupporting = ResolveIsSupporting(lesson)
                        };
'''
if old_item in service:
    service = service.replace(old_item, new_item, 1)
elif new_item not in service:
    raise SystemExit("FAIL LessonContentService library item anchor missing")
service = service.replace(
    '''                        LessonContentPolicy.IsSupporting(
                            lesson.OfficialOutcomeCount)))''',
    '''                        ResolveIsSupporting(lesson)))''')
service = service.replace(
    '''                LessonContentPolicy.IsSupporting(lesson.OfficialOutcomeCount))));''',
    '''                ResolveIsSupporting(lesson))));''')
helper_anchor = '''    private static string DisplayLevel(
        CanonicalCurriculumContextRecord context) =>
'''
helper = '''    private static bool ResolveIsSupporting(
        PedagogicalLessonRecord lesson) =>
        lesson.IsSupporting ??
        LessonContentPolicy.IsSupporting(
            lesson.OfficialOutcomeCount);

    private static string DisplayLevel(
        CanonicalCurriculumContextRecord context) =>
'''
if helper_anchor in service:
    service = service.replace(helper_anchor, helper, 1)
elif helper not in service:
    raise SystemExit("FAIL LessonContentService helper anchor missing")
write(service_path, service)

# Reclassify reviewed core scope sequences. This does NOT invent mappings.
content_dir = ROOT / "src/Edulytics.Core/Curriculum/LessonContent/Packs"
reclassified = 0
for path in sorted(content_dir.glob("*.lesson-content-pack.json")):
    name = path.name
    is_cambridge_core_sequence = (
        name.startswith("cambridge-primary-stage") and
        not name.startswith("cambridge-primary-stage1-")
    ) or name.startswith("cambridge-lower-stage") or name.startswith("cambridge-igcse-") or name.startswith("cambridge-as-level-") or name.startswith("cambridge-a-level-")
    is_uae_core_sequence = name.startswith("uae-g") and name.endswith("-ogl-v1.lesson-content-pack.json")
    if not (is_cambridge_core_sequence or is_uae_core_sequence):
        continue
    doc = json.loads(path.read_text(encoding="utf-8"))
    changed = False
    for lesson in doc.get("Lessons", doc.get("lessons", [])):
        key = "IsSupporting" if "IsSupporting" in lesson else "isSupporting"
        if lesson.get(key) is not False:
            lesson[key] = False
            changed = True
        status_key = "AdaptationStatus" if "AdaptationStatus" in lesson else "adaptationStatus"
        status = str(lesson.get(status_key, ""))
        if "supporting lesson" in status:
            lesson[status_key] = status.replace("supporting lesson", "curriculum lesson")
            changed = True
    if changed:
        path.write_text(json.dumps(doc, ensure_ascii=False, indent=2) + "\n", encoding="utf-8")
        reclassified += 1
print(f"PASS reclassified core curriculum content packs: {reclassified}")

# Keep rollout generators consistent with reviewed runtime role semantics.
for generator in [
    "tools/phase29/cambridge_primary_dfe_rollout.py",
    "tools/phase29/complete_remaining_curricula.py",
]:
    text = read(generator)
    text = text.replace('"IsSupporting": True,', '"IsSupporting": False,')
    text = text.replace("Edulytics-authored supporting lesson", "Edulytics-authored curriculum lesson")
    text = text.replace("supporting lesson informed by", "curriculum lesson informed by")
    write(generator, text)

# ---------------------------------------------------------------------------
# 2. Back navigation preserves the exact selected lesson-library context.
# ---------------------------------------------------------------------------
controller_path = "src/Edulytics.Web/Controllers/LessonContentController.cs"
controller = read(controller_path)
old_sig = '''    public async Task<IActionResult> Detail(
        Guid id,
        CancellationToken cancellationToken)
'''
new_sig = '''    public async Task<IActionResult> Detail(
        Guid id,
        Guid? academicYearId,
        Guid? academicProgramId,
        Guid? curriculumAdoptionId,
        CancellationToken cancellationToken)
'''
if old_sig in controller:
    controller = controller.replace(old_sig, new_sig, 1)
elif new_sig not in controller:
    raise SystemExit("FAIL LessonContentController Detail signature anchor missing")
view_anchor = '''        return View(
            new LessonContentDetailViewModel(
                result.Value));
'''
view_replacement = '''        ViewData["BackAcademicYearId"] = academicYearId;
        ViewData["BackAcademicProgramId"] = academicProgramId;
        ViewData["BackCurriculumAdoptionId"] = curriculumAdoptionId;

        return View(
            new LessonContentDetailViewModel(
                result.Value));
'''
if view_anchor in controller:
    controller = controller.replace(view_anchor, view_replacement, 1)
elif view_replacement not in controller:
    raise SystemExit("FAIL LessonContentController View anchor missing")
write(controller_path, controller)

detail_path = "src/Edulytics.Web/Views/LessonContent/Detail.cshtml"
detail = read(detail_path)
layout_anchor = '''    ViewData["Title"] = Model.Lesson.LessonTitle;
'''
layout_replacement = '''    ViewData["Title"] = Model.Lesson.LessonTitle;
    var backAcademicYearId = ViewData["BackAcademicYearId"] is Guid yearId ? yearId : (Guid?)null;
    var backAcademicProgramId = ViewData["BackAcademicProgramId"] is Guid programId ? programId : (Guid?)null;
    var backCurriculumAdoptionId = ViewData["BackCurriculumAdoptionId"] is Guid adoptionId ? adoptionId : (Guid?)null;
'''
if layout_anchor in detail:
    detail = detail.replace(layout_anchor, layout_replacement, 1)
elif layout_replacement not in detail:
    raise SystemExit("FAIL Detail.cshtml header anchor missing")
back_anchor = '''       asp-controller="LessonContent"
       asp-action="Index">
'''
back_replacement = '''       asp-controller="LessonContent"
       asp-action="Index"
       asp-route-academicYearId="@backAcademicYearId"
       asp-route-academicProgramId="@backAcademicProgramId"
       asp-route-curriculumAdoptionId="@backCurriculumAdoptionId">
'''
if back_anchor in detail:
    detail = detail.replace(back_anchor, back_replacement, 1)
elif back_replacement not in detail:
    raise SystemExit("FAIL Detail.cshtml back link anchor missing")
write(detail_path, detail)

# ---------------------------------------------------------------------------
# 3. Lesson library: only three KPIs, real supporting role, context-preserving links,
#    and a selector surface that matches the application card/form language.
# ---------------------------------------------------------------------------
index_path = "src/Edulytics.Web/Views/LessonContent/Index.cshtml"
index = read(index_path)
index = index.replace('class="lesson-content-filter"', 'class="lesson-content-filter app-surface-card"', 1)
index = index.replace(
    '<div class="lesson-content-filter__field">\n                <label for="academicYearId">',
    '<div class="lesson-content-filter__field">\n                <span class="lesson-content-filter__step" aria-hidden="true">1</span>\n                <label for="academicYearId">',
    1)
index = index.replace(
    '<div class="lesson-content-filter__field">\n                <label for="academicProgramId">',
    '<div class="lesson-content-filter__field">\n                <span class="lesson-content-filter__step" aria-hidden="true">2</span>\n                <label for="academicProgramId">',
    1)
index = index.replace(
    '<div class="lesson-content-filter__field">\n                <label for="curriculumAdoptionId">',
    '<div class="lesson-content-filter__field">\n                <span class="lesson-content-filter__step" aria-hidden="true">3</span>\n                <label for="curriculumAdoptionId">',
    1)
index = index.replace('<select id="academicYearId" name="academicYearId" required>', '<select id="academicYearId" name="academicYearId" class="lesson-content-select" required>', 1)
index = index.replace('<select id="academicProgramId" name="academicProgramId" required>', '<select id="academicProgramId" name="academicProgramId" class="lesson-content-select" required>', 1)
index = index.replace('<select id="curriculumAdoptionId" name="curriculumAdoptionId" required>', '<select id="curriculumAdoptionId" name="curriculumAdoptionId" class="lesson-content-select" required>', 1)
old_counts = '''                    var officiallyAlignedCount = group.Lessons.Count(x => x.HasOfficialAlignment);
                    var supportingCount = group.Lessons.Count - officiallyAlignedCount;
                    var coveragePercent = group.TotalLessons == 0
                        ? 0
                        : (int)Math.Round(100m * group.ProductionReadyLessons / group.TotalLessons);
'''
new_counts = '''                    var supportingCount = group.Lessons.Count(x => x.IsSupporting);
'''
if old_counts in index:
    index = index.replace(old_counts, new_counts, 1)
elif new_counts not in index:
    raise SystemExit("FAIL Index KPI count anchor missing")
# Remove coverage and officially-aligned metric cards.
index = re.sub(
    r'''\s*<div class="lesson-content-metric">\s*<span class="lesson-content-metric__value">@coveragePercent%</span>\s*<span class="lesson-content-metric__label">@L\["Coverage"\]</span>\s*</div>''',
    '', index, count=1)
index = re.sub(
    r'''\s*<div class="lesson-content-metric">\s*<span class="lesson-content-metric__value">@officiallyAlignedCount</span>\s*<span class="lesson-content-metric__label">@L\["OfficiallyAligned"\]</span>\s*</div>''',
    '', index, count=1)
index = index.replace('var isSupporting = !lesson.HasOfficialAlignment;', 'var isSupporting = lesson.IsSupporting;', 1)
link_anchor = '''                                       asp-action="Detail"
                                       asp-route-id="@lesson.LessonId">'''
link_replacement = '''                                       asp-action="Detail"
                                       asp-route-id="@lesson.LessonId"
                                       asp-route-academicYearId="@Model.Dashboard.SelectedAcademicYearId"
                                       asp-route-academicProgramId="@Model.Dashboard.SelectedAcademicProgramId"
                                       asp-route-curriculumAdoptionId="@Model.Dashboard.SelectedCurriculumAdoptionId">'''
if link_anchor in index:
    index = index.replace(link_anchor, link_replacement, 1)
elif link_replacement not in index:
    raise SystemExit("FAIL Index detail link anchor missing")
write(index_path, index)

# ---------------------------------------------------------------------------
# 4. Rich, curriculum-neutral lesson presentation: readable paragraphs plus a
#    deterministic instructional visual when a lesson has no explicit source visual.
# ---------------------------------------------------------------------------
parser_path = "src/Edulytics.Web/Presentation/LessonPresentationParser.cs"
parser = read(parser_path)
parser = parser.replace(
    '''    GeometricFigure
}''',
    '''    GeometricFigure,
    ConceptFlow
}''', 1)
parse_return_anchor = '''        return result;
    }

    private static string PrepareLearnerFacingText(
'''
parse_return_replacement = '''        if (
            sectionKind == "explanation" &&
            result.All(x => !x.IsVisual))
        {
            var fallback = TryCreateFallbackVisual(safe);
            if (fallback is not null)
            {
                result.Insert(
                    Math.Min(1, result.Count),
                    new LessonPresentationItem(null, fallback));
            }
        }

        return result;
    }

    private static string PrepareLearnerFacingText(
'''
if parse_return_anchor in parser:
    parser = parser.replace(parse_return_anchor, parse_return_replacement, 1)
elif parse_return_replacement not in parser:
    raise SystemExit("FAIL parser Parse return anchor missing")

old_split = '''        if (
            sectionKind is
                "explanation" or
                "concepts" or
                "examples" or
                "mistakes")
        {
            return value
                .Split(
                    '\\n',
                    StringSplitOptions.RemoveEmptyEntries |
                    StringSplitOptions.TrimEntries)
                .Where(
                    x =>
                        !string.IsNullOrWhiteSpace(x))
                .ToArray();
        }
'''
new_split = '''        if (
            sectionKind is
                "explanation" or
                "concepts" or
                "examples" or
                "mistakes")
        {
            var sourceBlocks = value
                .Split(
                    '\\n',
                    StringSplitOptions.RemoveEmptyEntries |
                    StringSplitOptions.TrimEntries)
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .ToArray();

            var readable = new List<string>();
            foreach (var sourceBlock in sourceBlocks)
            {
                var sentences = SentenceBoundaryRegex
                    .Split(sourceBlock)
                    .Select(x => x.Trim())
                    .Where(x => x.Length > 0)
                    .ToArray();

                if (sentences.Length <= 2)
                {
                    readable.Add(sourceBlock);
                    continue;
                }

                for (var i = 0; i < sentences.Length; i += 2)
                {
                    readable.Add(
                        string.Join(
                            " ",
                            sentences.Skip(i).Take(2)));
                }
            }

            return readable;
        }
'''
if old_split in parser:
    parser = parser.replace(old_split, new_split, 1)
elif new_split not in parser:
    raise SystemExit("FAIL parser readable split anchor missing")

fallback_method = r'''
    private static LessonVisualSpec? TryCreateFallbackVisual(
        string value)
    {
        var plain = NormalizeText(value);
        if (string.IsNullOrWhiteSpace(plain))
            return null;

        var lower = plain.ToLowerInvariant();
        var polish = Regex.IsMatch(
            lower,
            @"[ąćęłńóśźż]|\b(?:liczb|ułam|kąt|figur|pole|wykres|współrzęd|proporcj)" +
            @"",
            RegexOptions.CultureInvariant);

        var title = polish
            ? "Model pojęcia"
            : "Concept model";
        var accessibility = polish
            ? "Schemat wspierający rozumienie pojęcia matematycznego."
            : "Instructional diagram supporting the mathematical concept.";

        if (ContainsAny(lower,
                "coordinate", "coordinates", "graph", "function", "współrzęd", "wykres", "funkcj"))
        {
            return new(
                LessonVisualType.CoordinatePlane,
                accessibility,
                title,
                string.Empty,
                string.Empty,
                [], [], [], [],
                "concept-coordinate-plane");
        }

        if (ContainsAny(lower,
                "area", "perimeter", "geometry", "shape", "angle", "triangle", "polygon",
                "pole", "obwód", "geometri", "figur", "kąt", "trójkąt", "wielokąt"))
        {
            return new(
                LessonVisualType.AreaDecomposition,
                accessibility,
                title,
                string.Empty,
                string.Empty,
                [], [], [], [],
                "concept-area-decomposition");
        }

        if (ContainsAny(lower,
                "number", "integer", "fraction", "decimal", "percent", "ratio", "proportion", "rate",
                "liczb", "ułam", "dziesięt", "procent", "stosunk", "proporcj"))
        {
            return new(
                LessonVisualType.NumberLine,
                accessibility,
                title,
                string.Empty,
                string.Empty,
                [], [], [], [],
                "concept-number-line");
        }

        return new(
            LessonVisualType.ConceptFlow,
            accessibility,
            title,
            string.Empty,
            string.Empty,
            [],
            [],
            polish
                ? ["Zrozum", "Przedstaw", "Sprawdź"]
                : ["Understand", "Represent", "Check"],
            [],
            "concept-flow");
    }

    private static bool ContainsAny(
        string value,
        params string[] terms) =>
        terms.Any(term => value.Contains(term, StringComparison.OrdinalIgnoreCase));

'''
visual_anchor = '''    private static LessonVisualSpec? TryCreateVisual(
        string description)
'''
if fallback_method not in parser:
    if visual_anchor not in parser:
        raise SystemExit("FAIL parser visual anchor missing")
    parser = parser.replace(visual_anchor, fallback_method + visual_anchor, 1)
write(parser_path, parser)

partial_path = "src/Edulytics.Web/Views/Shared/_LessonInstructionalVisual.cshtml"
partial = read(partial_path)
concept_branch = r'''
    else if (visual.Type == LessonVisualType.ConceptFlow)
    {
        var labels = visual.Labels.Count >= 3
            ? visual.Labels
            : new[] { "Understand", "Represent", "Check" };

        <svg viewBox="0 0 760 250"
             role="img"
             aria-label="@visual.AccessibilityText">

            <title>@visual.AccessibilityText</title>

            <path d="M220 125h95 M445 125h95"
                  class="visual-arrow" />

            @for (var i = 0; i < 3; i++)
            {
                var x = 35 + i * 250;

                <rect x="@x"
                      y="72"
                      width="180"
                      height="106"
                      rx="24"
                      class="visual-area-fill" />

                <foreignObject x="@(x + 15)"
                               y="100"
                               width="150"
                               height="54">
                    <div xmlns="http://www.w3.org/1999/xhtml"
                         class="visual-svg-concept">
                        @labels[i]
                    </div>
                </foreignObject>
            }
        </svg>
    }
'''
last_marker = '''    else if (visual.Type == LessonVisualType.GeometricFigure)
'''
if concept_branch not in partial:
    if last_marker not in partial:
        raise SystemExit("FAIL instructional visual geometric anchor missing")
    partial = partial.replace(last_marker, concept_branch + last_marker, 1)
write(partial_path, partial)

# ---------------------------------------------------------------------------
# 5. Visual/UI polish for selector and 3-card metrics.
# ---------------------------------------------------------------------------
css_path = "src/Edulytics.Web/wwwroot/css/site.css"
css = read(css_path)
css_marker = "/* Phase 29 final acceptance — lesson library selector parity */"
if css_marker not in css:
    css += r'''

/* Phase 29 final acceptance — lesson library selector parity */
.lesson-content-filter.app-surface-card {
    display: grid;
    grid-template-columns: repeat(3, minmax(0, 1fr)) auto;
    gap: 1rem;
    align-items: end;
    padding: 1.15rem;
    margin: 1.25rem 0 1.5rem;
    border: 1px solid var(--app-border, #dfe5f1);
    border-radius: 18px;
    background: var(--app-surface, #fff);
    box-shadow: 0 10px 30px rgba(25, 39, 75, .06);
}

.lesson-content-filter__field {
    position: relative;
    display: grid;
    gap: .45rem;
    min-width: 0;
}

.lesson-content-filter__field label {
    padding-left: 2rem;
    font-size: .78rem;
    font-weight: 800;
    letter-spacing: .035em;
    text-transform: uppercase;
    color: #475569;
}

.lesson-content-filter__step {
    position: absolute;
    top: -.12rem;
    left: 0;
    display: inline-grid;
    place-items: center;
    width: 1.5rem;
    height: 1.5rem;
    border-radius: 999px;
    background: #eef2ff;
    color: #4f46e5;
    font-size: .74rem;
    font-weight: 900;
}

.lesson-content-select {
    width: 100%;
    min-height: 46px;
    padding: .68rem 2.35rem .68rem .82rem;
    border: 1px solid #cbd5e1;
    border-radius: 12px;
    background: #fff;
    color: #0f172a;
    font: inherit;
    font-weight: 650;
    outline: none;
    transition: border-color .15s ease, box-shadow .15s ease;
}

.lesson-content-select:focus {
    border-color: #6366f1;
    box-shadow: 0 0 0 3px rgba(99, 102, 241, .14);
}

.lesson-content-filter__action .school-button {
    min-height: 46px;
    padding-inline: 1.25rem;
    border-radius: 12px;
    white-space: nowrap;
}

.lesson-content-metrics {
    grid-template-columns: repeat(3, minmax(0, 1fr));
}

.lesson-reader-blocks--explanation,
.lesson-reader-blocks--concepts,
.lesson-reader-blocks--examples,
.lesson-reader-blocks--mistakes {
    gap: 1rem;
}

.lesson-reader-text {
    line-height: 1.75;
}

.lesson-instructional-visual {
    margin-block: 1rem 1.25rem;
}

@media (max-width: 1050px) {
    .lesson-content-filter.app-surface-card {
        grid-template-columns: 1fr 1fr;
    }
}

@media (max-width: 680px) {
    .lesson-content-filter.app-surface-card,
    .lesson-content-metrics {
        grid-template-columns: 1fr;
    }

    .lesson-content-filter__action .school-button {
        width: 100%;
    }
}
'''
write(css_path, css)

# ---------------------------------------------------------------------------
# 6. Regression tests for the acceptance feedback.
# ---------------------------------------------------------------------------
test = r'''using Edulytics.Core.Curriculum;
using Edulytics.Web.Presentation;
using Xunit;

namespace Edulytics.Tests.Phase29;

public sealed class Phase29FinalAcceptanceTests
{
    [Fact]
    public void CambridgeCoreSequence_IsNotMislabelledSupporting()
    {
        const string code =
            "PED:CAMBRIDGE-INTL-MATH:L7:SHARED:01:01:INTEGERS-AND-DIRECTED-NUMBER";

        Assert.True(
            CanonicalLessonRoleRegistry.TryGetIsSupporting(
                code,
                out var supporting));
        Assert.False(supporting);
    }

    [Fact]
    public void PolishNumberLesson_GetsDeterministicInstructionalVisual()
    {
        var items = LessonPresentationParser.Parse(
            "Lekcja dotyczy posługiwania się liczbami. Uczeń rozpoznaje relacje i sprawdza wynik działaniem odwrotnym.",
            sectionKind: "explanation");

        Assert.Contains(
            items,
            x => x.VisualType == LessonVisualType.NumberLine);
    }

    [Fact]
    public void GenericAlgebraLesson_GetsConceptFlowInsteadOfNoVisual()
    {
        var items = LessonPresentationParser.Parse(
            "Solve an algebraic expression by preserving equality and checking the result by substitution.",
            sectionKind: "explanation");

        Assert.Contains(
            items,
            x => x.VisualType == LessonVisualType.ConceptFlow);
    }

    [Fact]
    public void LessonLibraryView_PreservesContextAndShowsOnlyRequestedKpis()
    {
        var root = FindRepositoryRoot();
        var source = File.ReadAllText(
            Path.Combine(
                root,
                "src/Edulytics.Web/Views/LessonContent/Index.cshtml"));

        Assert.Contains("asp-route-academicYearId", source, StringComparison.Ordinal);
        Assert.Contains("asp-route-academicProgramId", source, StringComparison.Ordinal);
        Assert.Contains("asp-route-curriculumAdoptionId", source, StringComparison.Ordinal);
        Assert.Contains("@L[\"TotalLessons\"]", source, StringComparison.Ordinal);
        Assert.Contains("@L[\"ProductionReady\"]", source, StringComparison.Ordinal);
        Assert.Contains("@L[\"SupportingLessons\"]", source, StringComparison.Ordinal);
        Assert.DoesNotContain("@L[\"Coverage\"]", source, StringComparison.Ordinal);
        Assert.DoesNotContain("@L[\"OfficiallyAligned\"]", source, StringComparison.Ordinal);
        Assert.Contains("var isSupporting = lesson.IsSupporting;", source, StringComparison.Ordinal);
    }

    [Fact]
    public void LessonDetailBackLink_PreservesExactLibraryContext()
    {
        var root = FindRepositoryRoot();
        var source = File.ReadAllText(
            Path.Combine(
                root,
                "src/Edulytics.Web/Views/LessonContent/Detail.cshtml"));

        Assert.Contains("asp-route-academicYearId", source, StringComparison.Ordinal);
        Assert.Contains("asp-route-academicProgramId", source, StringComparison.Ordinal);
        Assert.Contains("asp-route-curriculumAdoptionId", source, StringComparison.Ordinal);
    }

    private static string FindRepositoryRoot()
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);
        while (directory is not null)
        {
            if (File.Exists(Path.Combine(directory.FullName, "Edulytics.sln")))
                return directory.FullName;
            directory = directory.Parent;
        }

        throw new InvalidOperationException("Repository root was not found.");
    }
}
'''
write("tests/Edulytics.Tests/Phase29/Phase29FinalAcceptanceTests.cs", test)

print("PASS: Phase 29 final acceptance patch staged in working tree")
