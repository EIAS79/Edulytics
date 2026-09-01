#!/usr/bin/env python3
from pathlib import Path

p = Path("src/Edulytics.Web/Controllers/LessonContentController.cs")
text = p.read_text(encoding="utf-8")
old = '''    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Detail(Guid id,CancellationToken cancellationToken)
    {
        if(!TryActor(out var actorId))return Forbid();
        var r=await _lessons.GetStaffLessonAsync(actorId,id,CultureInfo.CurrentUICulture.Name,cancellationToken);
        return r.Value is null?HandleError(r.Error):View(new LessonContentDetailViewModel(r.Value));
    }
'''
new = '''    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Detail(
        Guid id,
        Guid? academicYearId,
        Guid? academicProgramId,
        Guid? curriculumAdoptionId,
        CancellationToken cancellationToken)
    {
        if(!TryActor(out var actorId))return Forbid();
        var result=await _lessons.GetStaffLessonAsync(actorId,id,CultureInfo.CurrentUICulture.Name,cancellationToken);
        if (result.Value is null)
            return HandleError(result.Error);

        ViewData["BackAcademicYearId"] = academicYearId;
        ViewData["BackAcademicProgramId"] = academicProgramId;
        ViewData["BackCurriculumAdoptionId"] = curriculumAdoptionId;

        return View(
            new LessonContentDetailViewModel(
                result.Value));
    }
'''
if old in text:
    p.write_text(text.replace(old, new, 1), encoding="utf-8")
elif new not in text:
    raise SystemExit("FAIL: compact LessonContentController Detail block not found")
print("PASS: detail controller accepts and preserves lesson-library context")
