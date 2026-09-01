#!/usr/bin/env python3
from pathlib import Path

p = Path("src/Edulytics.Web/Views/LessonContent/Detail.cshtml")
text = p.read_text(encoding="utf-8")
for line in [
    '    var backAcademicYearId = ViewData["BackAcademicYearId"] is Guid yearId ? yearId : (Guid?)null;\n',
    '    var backAcademicProgramId = ViewData["BackAcademicProgramId"] is Guid programId ? programId : (Guid?)null;\n',
    '    var backCurriculumAdoptionId = ViewData["BackCurriculumAdoptionId"] is Guid adoptionId ? adoptionId : (Guid?)null;\n',
]:
    text = text.replace(line, "")
expanded = '''    <a class="lesson-reader-back"
       asp-controller="LessonContent"
       asp-action="Index"
       asp-route-academicYearId="@backAcademicYearId"
       asp-route-academicProgramId="@backAcademicProgramId"
       asp-route-curriculumAdoptionId="@backCurriculumAdoptionId">
'''
base = '''    <a class="lesson-reader-back"
       asp-controller="LessonContent"
       asp-action="Index">
'''
if expanded in text:
    text = text.replace(expanded, base, 1)
elif base not in text:
    raise SystemExit("FAIL: lesson detail back-link shape is unexpected")
p.write_text(text, encoding="utf-8")
print("PASS: lesson detail prepared for idempotent main acceptance patch")
