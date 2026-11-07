from pathlib import Path
import subprocess, json
root=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
def from_git(path):
    return subprocess.check_output(["git","-C",str(root),"show","HEAD:"+path],encoding="utf-8-sig")
def to_repo(path,text):
    out=root/path
    out.parent.mkdir(parents=True,exist_ok=True)
    out.write_text(text,encoding="utf-8",newline="\n")
def once(text,old,new):
    count=text.count(old)
    assert count==1, ("Expected one anchor",old[:100],count)
    return text.replace(old,new)

oldsvc="src/Edulytics.Web/Services/Grade11Algebra2RubricService.cs"
service=from_git(oldsvc)
service=service.replace("Grade11Algebra2","Grade12Precalculus").replace("G11Alg2","G12Precalc")
service=service.replace("Course-owned G11 Algebra 2 teacher-scored rubrics","Course-owned G12 Precalculus per-exercise teacher-scored rubrics")
service=once(service,
    'Guid LessonId, string ResponseText, DateTime SubmittedAtUtc,',
    'Guid LessonId, int ExerciseNumber, string ResponseText, DateTime SubmittedAtUtc,')
service=once(service,
    'Guid LessonId, Guid CurriculumAdoptionId, G12PrecalcRubricRow? Existing',
    'Guid LessonId, int ExerciseNumber, string Prompt, G12PrecalcRubricRow? Existing')
service=service.replace('public const string Prefix = "PED:US-CCSS-MATH:HS:TRAD:ALG2:IM-1E:";',
'''public const string LessonCode = "PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:U01:L01";''')
assert "Prefix" not in service.split('internal static class G12PrecalcRubricStorage')[1].split('private static DbParameter')[0]
service=service.replace('"US-CCSS-MATH:L11:TRADITIONAL:COURSE-ALG2"', '"US-CCSS-MATH:L13:ADVANCED-PRECALCULUS:COURSE-PRECALC"')
service=once(service,'"Mathematical functions, variables, equations, data or graphical models are represented accurately",',
  '"The exercise-specific function, graph, table or algebraic model is represented accurately",')
service=once(service,'"Calculations, symbolic transformations and stated constraints are mathematically correct",',
  '"Calculations, solution sets, domain restrictions and graph intersections are mathematically correct",')
service=once(service,'"A complete explanation justifies the reasoning, domain assumptions, and conclusions",',
  '"Mathematical justification and all requested source-skill subparts are complete",')
service=once(service,'"Written labels, units, interpretation and any teacher-inspected evidence meet the lesson goals"',
  '"Axes, labels, precision, physical units and teacher-inspected diagrams meet Section 1.1 goals"')
service=once(service,
    'r.GetGuid(0), r.GetGuid(1), r.GetGuid(2), r.GetGuid(3), r.GetGuid(4),\n        r.GetString(5), r.GetDateTime(6),\n        r.IsDBNull(7) ? null : r.GetGuid(7),\n        r.IsDBNull(8) ? null : r.GetInt32(8),\n        r.IsDBNull(9) ? null : r.GetString(9),\n        r.IsDBNull(10) ? null : r.GetDateTime(10));',
    'r.GetGuid(0), r.GetGuid(1), r.GetGuid(2), r.GetGuid(3), r.GetGuid(4),\n        r.GetInt32(5), r.GetString(6), r.GetDateTime(7),\n        r.IsDBNull(8) ? null : r.GetGuid(8),\n        r.IsDBNull(9) ? null : r.GetInt32(9),\n        r.IsDBNull(10) ? null : r.GetString(10),\n        r.IsDBNull(11) ? null : r.GetDateTime(11));')
service=once(service,
    '"CurriculumPedagogicalLessonId","ResponseText","SubmittedAtUtc",',
    '"CurriculumPedagogicalLessonId","ExerciseNumber","ResponseText","SubmittedAtUtc",')
# Storage FindStudentAsync signature and service wrapper (both occurrences)
assert service.count('Guid lesson, CancellationToken ct)')==2
service=service.replace('Guid lesson, CancellationToken ct)', 'Guid lesson, int exerciseNumber, CancellationToken ct)')
service=once(service,
    'AND "CurriculumPedagogicalLessonId"=@lesson\n                """',
    'AND "CurriculumPedagogicalLessonId"=@lesson\n                   AND "ExerciseNumber"=@exercise\n                """')
service=once(service,
    '("student", student), ("lesson", lesson));',
    '("student", student), ("lesson", lesson), ("exercise", exerciseNumber));')
# Storage and wrapper InsertAsync
assert service.count('Guid lesson, string response, CancellationToken ct)')==1
service=service.replace('Guid lesson, string response, CancellationToken ct)', 'Guid lesson, int exerciseNumber, string response, CancellationToken ct)')
service=once(service,
    '"CurriculumPedagogicalLessonId","ResponseText","SubmittedAtUtc")',
    '"CurriculumPedagogicalLessonId","ExerciseNumber","ResponseText","SubmittedAtUtc")')
service=once(service,
    'VALUES (@id,@school,@group,@student,@lesson,@response,@submitted)',
    'VALUES (@id,@school,@group,@student,@lesson,@exercise,@response,@submitted)')
service=once(service,
    '"CurriculumPedagogicalLessonId") DO NOTHING',
    '"CurriculumPedagogicalLessonId","ExerciseNumber") DO NOTHING')
service=once(service,
    '("student", student), ("lesson", lesson),\n            ("response", response)',
    '("student", student), ("lesson", lesson), ("exercise", exerciseNumber),\n            ("response", response)')
# Service wrapper signatures are split across two lines
service=once(service,
    'Guid lesson, CancellationToken ct) =>\n        G12PrecalcRubricStorage.FindStudentAsync(db, school, group, student, lesson, ct);',
    'Guid lesson, int exerciseNumber, CancellationToken ct) =>\n        G12PrecalcRubricStorage.FindStudentAsync(db, school, group, student, lesson, exerciseNumber, ct);') if 'Guid lesson, CancellationToken ct) =>' in service else service
service=once(service,
    'G12PrecalcRubricStorage.FindStudentAsync(db, school, group, student, lesson, ct);',
    'G12PrecalcRubricStorage.FindStudentAsync(db, school, group, student, lesson, exerciseNumber, ct);') if 'G12PrecalcRubricStorage.FindStudentAsync(db, school, group, student, lesson, ct);' in service else service
service=once(service,
    'Guid lesson, string response, CancellationToken ct) =>\n        G12PrecalcRubricStorage.InsertAsync(db, school, group, student, lesson, response, ct);',
    'Guid lesson, int exerciseNumber, string response, CancellationToken ct) =>\n        G12PrecalcRubricStorage.InsertAsync(db, school, group, student, lesson, exerciseNumber, response, ct);') if 'Guid lesson, string response, CancellationToken ct) =>' in service else service
service=once(service,
    'G12PrecalcRubricStorage.InsertAsync(db, school, group, student, lesson, response, ct);',
    'G12PrecalcRubricStorage.InsertAsync(db, school, group, student, lesson, exerciseNumber, response, ct);') if 'G12PrecalcRubricStorage.InsertAsync(db, school, group, student, lesson, response, ct);' in service else service
service=once(service,
    'Guid school, Guid group, Guid student, Guid lesson,\n        string response, CancellationToken ct) =>',
    'Guid school, Guid group, Guid student, Guid lesson,\n        int exerciseNumber, string response, CancellationToken ct) =>')
service=service.replace('lesson.Code.StartsWith(G12PrecalcRubricStorage.Prefix)','lesson.Code == G12PrecalcRubricStorage.LessonCode')
assert "G12PrecalcRubricStorage.Prefix" not in service
assert 'LessonCode' in service
to_repo("src/Edulytics.Web/Services/Grade12PrecalculusRubricService.cs",service)

controller=from_git("src/Edulytics.Web/Controllers/Grade11Algebra2RubricController.cs")
controller=controller.replace("Grade11Algebra2","Grade12Precalculus").replace("G11Alg2","G12Precalc")
controller=controller.replace('"student/g11/algebra2/rubric"','"student/g12/precalculus/rubric"').replace('"school/g11/algebra2/rubric"','"school/g12/precalculus/rubric"')
controller=once(controller,'using Edulytics.Core.Constants;','using Edulytics.Core.Constants;\nusing Edulytics.Web.Presentation;')
controller=once(controller,'Guid lessonId, CancellationToken ct)\n    {\n        if (!Actor(out var actor)) return Forbid();',
'''Guid lessonId, int exerciseNumber, CancellationToken ct)
    {
        if (!Actor(out var actor)) return Forbid();
        var task = Grade12PrecalculusSection11Exercises.All
            .FirstOrDefault(item => item.Number == exerciseNumber);
        if (task is null) return NotFound();''')
controller=once(controller,
    'rubric.FindStudentAsync( scope.Value.School, scope.Value.Class, scope.Value.Student, lessonId, ct);',
    'rubric.FindStudentAsync(scope.Value.School, scope.Value.Class, scope.Value.Student, lessonId, exerciseNumber, ct);')
controller=once(controller,
    'new G12PrecalcRubricStudentPage(lessonId, Guid.Empty, existing)',
    'new G12PrecalcRubricStudentPage(lessonId, exerciseNumber, task.Prompt, existing)')
controller=once(controller,
    'Guid lessonId,\n        [FromForm] string? explanation, CancellationToken ct)',
    'Guid lessonId, int exerciseNumber,\n        [FromForm] string? explanation, CancellationToken ct)')
controller=once(controller,
    '''        if (!Actor(out var actor)) return Forbid();
        var scope = await rubric.StudentScopeAsync(actor, lessonId, Guid.Empty, ct);
        if (scope is null) return NotFound();
        if (string.IsNullOrWhiteSpace(explanation)''',
    '''        if (!Actor(out var actor)) return Forbid();
        if (!Grade12PrecalculusSection11Exercises.All.Any(x => x.Number == exerciseNumber))
            return NotFound();
        var scope = await rubric.StudentScopeAsync(actor, lessonId, Guid.Empty, ct);
        if (scope is null) return NotFound();
        if (string.IsNullOrWhiteSpace(explanation)''')
controller=once(controller,
    'lessonId, explanation.Trim(), ct);',
    'lessonId, exerciseNumber, explanation.Trim(), ct);')
controller=once(controller,
    'return RedirectToAction(nameof(Index), new { lessonId });',
    'return RedirectToAction(nameof(Index), new { lessonId, exerciseNumber });')
to_repo("src/Edulytics.Web/Controllers/Grade12PrecalculusRubricController.cs",controller)

migration=from_git("src/Edulytics.Data/Migrations/20261106050000_Grade11Algebra2TeacherRubric.cs")
migration=migration.replace("Grade11Algebra2","Grade12Precalculus").replace("G11Algebra2","G12Precalc")
migration=migration.replace('20261106050000_Grade12PrecalculusTeacherRubric','20261106193000_Grade12PrecalculusTeacherRubric')
migration=migration.replace("G10 Geometry teacher-scored lesson work","G12 Precalculus Section 1.1 exercise-scoped teacher-graded work")
migration=once(migration,
    'CurriculumPedagogicalLessonId = t.Column<Guid>(type: "uuid", nullable: false),',
    'CurriculumPedagogicalLessonId = t.Column<Guid>(type: "uuid", nullable: false),\n                ExerciseNumber = t.Column<int>(type: "integer", nullable: false),')
migration=once(migration,
    'columns: new[] { "SchoolId", "ClassGroupId", "StudentProfileId", "CurriculumPedagogicalLessonId" },',
    'columns: new[] { "SchoolId", "ClassGroupId", "StudentProfileId", "CurriculumPedagogicalLessonId", "ExerciseNumber" },')
migration=once(migration,
    'migrationBuilder.Sql("""\n            ALTER TABLE "Grade12PrecalculusTeacherRubrics"',
    '''migrationBuilder.Sql("""
            ALTER TABLE "Grade12PrecalculusTeacherRubrics"''')
migration=once(migration,
    '''            """);
    }

    protected override void Down(MigrationBuilder migrationBuilder)''',
    '''            """);
        migrationBuilder.Sql("""
            ALTER TABLE "Grade12PrecalculusTeacherRubrics"
            ADD CONSTRAINT "CK_G12PrecalcRubric_ExerciseNumberRange"
            CHECK ("ExerciseNumber" BETWEEN 1 AND 92);
            """);
    }

    protected override void Down(MigrationBuilder migrationBuilder)''')
to_repo("src/Edulytics.Data/Migrations/20261106193000_Grade12PrecalculusTeacherRubric.cs",migration)

program=from_git("src/Edulytics.Web/Program.cs")
program=once(program,
    'builder.Services.AddScoped<Edulytics.Web.Controllers.Grade11Algebra2RubricService>();',
    'builder.Services.AddScoped<Edulytics.Web.Controllers.Grade11Algebra2RubricService>();\n'
    'builder.Services.AddScoped<Edulytics.Web.Controllers.Grade12PrecalculusRubricService>();')
to_repo("src/Edulytics.Web/Program.cs",program)

student_view='''@model Edulytics.Web.Controllers.G12PrecalcRubricStudentPage
@{ ViewData["Title"] = "Precalculus — Section 1.1 exercise response"; }
<h1>Precalculus: teacher-reviewed mathematical response</h1>
<p><strong>Exercise @Model.ExerciseNumber</strong>: @Model.Prompt</p>
<p>Explain each mathematical step and provide any requested graph with labelled axes to your teacher.
   Submit your written reasoning here. The platform does not automatically mark
   open-ended graphs or proofs.</p>
@if (Model.Existing is null)
{
    <form method="post" action="/student/g12/precalculus/rubric/submit">
        @Html.AntiForgeryToken()
        <input type="hidden" name="lessonId" value="@Model.LessonId" />
        <input type="hidden" name="exerciseNumber" value="@Model.ExerciseNumber" />
        <label for="explanation">Your independent answer and justification (20–8000 characters)</label>
        <textarea id="explanation" name="explanation" required minlength="20"
                  maxlength="8000" rows="10"></textarea>
        <button type="submit">Submit this exercise to the teacher</button>
    </form>
}
else
{
    <h2>Submitted response</h2>
    <p>Submitted @Model.Existing.SubmittedAtUtc.ToString("u")</p>
    <pre style="white-space:pre-wrap">@Model.Existing.ResponseText</pre>
    @if (Model.Existing.TeacherScore is int score)
    {
        <p>Teacher-awarded score: @score / 4</p>
        <p>Teacher feedback: @Model.Existing.TeacherFeedback</p>
    }
    else
    {
        <p>Teacher review pending. No automatic grade has been awarded.</p>
    }
}
'''
to_repo("src/Edulytics.Web/Views/Grade12PrecalculusRubricStudent/Index.cshtml",student_view)

teacher_index='''@model Edulytics.Web.Controllers.G12PrecalcRubricRow[]
@{ ViewData["Title"] = "Precalculus — teacher review queue"; }
<h1>Precalculus — teacher review queue</h1>
<p>Only submissions from students enrolled in your assigned mathematics groups
   are eligible for review. Score the particular exercise, not the complete section
   or the student's machine-graded Practice mastery.</p>
<ul>
@foreach (var row in Model)
{
    <li>
        <a href="/school/g12/precalculus/rubric/@row.Id">
            Exercise @row.ExerciseNumber · Lesson @row.LessonId · Student @row.StudentProfileId
        </a>
        — @row.SubmittedAtUtc.ToString("u")
        — @(row.ReviewedAtUtc.HasValue ? "Reviewed" : "Pending review")
    </li>
}
</ul>
'''
to_repo("src/Edulytics.Web/Views/Grade12PrecalculusRubricTeacher/Index.cshtml",teacher_index)

teacher_review='''@model Edulytics.Web.Controllers.G12PrecalcRubricTeacherPage
@{ ViewData["Title"] = "Precalculus — teacher assessment"; }
<h1>Section 1.1, exercise @Model.Submission.ExerciseNumber — teacher assessment</h1>
<p>Student: @Model.Submission.StudentProfileId</p>
<p>Submitted @Model.Submission.SubmittedAtUtc.ToString("u")</p>
<pre style="white-space:pre-wrap">@Model.Submission.ResponseText</pre>
@if (Model.Submission.TeacherScore is int score)
{
    <p>Teacher score: @score / 4</p>
    <p>Feedback: @Model.Submission.TeacherFeedback</p>
}
else
{
    <p>Inspect original reasoning, domain restrictions, all requested subparts and
       any separately submitted graph. Score each of four criteria using your own
       judgment; the platform does not grade drawings or explanations automatically.</p>
    <form method="post" action="/school/g12/precalculus/rubric/@Model.Submission.Id">
        @Html.AntiForgeryToken()
        @for (var i = 0; i < Model.Criteria.Length; i++)
        {
            <div>
                <label for="criterion-@i">@(i + 1). @Model.Criteria[i]</label>
                <select id="criterion-@i" name="marks" required>
                    <option value="">Choose a mark</option>
                    <option value="0">0 — Not demonstrated</option>
                    <option value="1">1 — Demonstrated</option>
                </select>
            </div>
        }
        <label for="teacherFeedback">Teacher feedback (10–2000 characters)</label>
        <textarea id="teacherFeedback" name="teacherFeedback" required minlength="10"
                  maxlength="2000" rows="5"></textarea>
        <button type="submit">Save teacher assessment</button>
    </form>
}
<a href="/school/g12/precalculus/rubric">Back to review queue</a>
'''
to_repo("src/Edulytics.Web/Views/Grade12PrecalculusRubricTeacher/Review.cshtml",teacher_review)

lesson_path="src/Edulytics.Web/Views/StudentPortal/Lesson.cshtml"
view=(root/lesson_path).read_text(encoding="utf-8")
anchor='''                                </li>
                            }
                        </ol>
                    </details>'''
assert view.count(anchor)==1,view.count(anchor)
view=once(view,anchor,'''                                    <a asp-controller="Grade12PrecalculusRubricStudent"
                                       asp-action="Index" asp-route-lessonId="@Model.Id"
                                       asp-route-exerciseNumber="@task.Number">
                                        Submit exercise @task.Number for teacher review
                                    </a>
                                </li>
                            }
                        </ol>
                    </details>''')
to_repo(lesson_path,view)

testpath="tests/Edulytics.Tests/MathematicsIntelligence/Grade12PrecalculusSection11SourceContractsTests.cs"
test=(root/testpath).read_text(encoding="utf-8")
assert "Grade12RubricRoutesAreLessonScopedAndPreserve92ExerciseNumbers" not in test
anchor='''    [Fact]
    public void IndependentWorkedExamplesHaveCorrectMathGoldenValues()'''
assert test.count(anchor)==1
test=test.replace(anchor,'''    [Fact]
    public void Grade12RubricRoutesAreLessonScopedAndPreserve92ExerciseNumbers()
    {
        Assert.Equal(92, Grade12PrecalculusSection11Exercises.All.Count);
        var studentRoute = Attribute.GetCustomAttribute(
            typeof(Edulytics.Web.Controllers.Grade12PrecalculusRubricStudentController),
            typeof(Microsoft.AspNetCore.Mvc.RouteAttribute)) as Microsoft.AspNetCore.Mvc.RouteAttribute;
        var teacherRoute = Attribute.GetCustomAttribute(
            typeof(Edulytics.Web.Controllers.Grade12PrecalculusRubricTeacherController),
            typeof(Microsoft.AspNetCore.Mvc.RouteAttribute)) as Microsoft.AspNetCore.Mvc.RouteAttribute;
        Assert.Equal("student/g12/precalculus/rubric", studentRoute?.Template);
        Assert.Equal("school/g12/precalculus/rubric", teacherRoute?.Template);
        // Tenant-scoped persistence and authorization are verified in Stage D.
    }

'''+anchor)
to_repo(testpath,test)

base=root/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
asspath=base/"u01-l01-assessment.json"
assessment=json.loads(asspath.read_text(encoding="utf-8"))
assessment["FirstPartySectionExerciseBank"]["TeacherSubmissionPersistenceContract"]="G12_EXERCISE_SCOPED_SCHOOL_CLASS_STUDENT_LESSON_EXERCISE_TABLE_AUTHORED_NOT_MIGRATED"
assessment["FirstPartySectionExerciseBank"]["TeacherSubmissionView"]="src/Edulytics.Web/Views/Grade12PrecalculusRubricStudent/Index.cshtml"
assessment["FirstPartySectionExerciseBank"]["TeacherEvaluationView"]="src/Edulytics.Web/Views/Grade12PrecalculusRubricTeacher/Review.cshtml"
assessment["FirstPartySectionExerciseBank"]["StudentTeacherRoutes"]="AUTHORED_NOT_RUNTIME_TESTED"
assessment["FirstPartySectionExerciseBank"]["DatabaseMigration"]="WRITTEN_NOT_APPLIED_DURING_A_B_C"
asspath.write_text(json.dumps(assessment,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")

checkpoint_path=base/"u01-l01-abc-checkpoint.json"
cp=json.loads(checkpoint_path.read_text(encoding="utf-8"))
cp["TeacherRubricSubmissionPersistence"]="EXERCISE_SCOPED_CONTROLLER_SERVICE_SQL_MIGRATION_AND_VIEWS_AUTHORED_STAGE_D_UNTESTED"
cp["TeacherRubricMigration"]="20261106193000_Grade12PrecalculusTeacherRubric"
cp["ReadyForStageD"]=False
checkpoint_path.write_text(json.dumps(cp,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")

progresspath=base/"full-grade-progress.json"
pg=json.loads(progresspath.read_text(encoding="utf-8"))
pg["LessonEntries"][0]["TeacherRubricSubmissionPersistence"]="G12_PER_EXERCISE_AUTHORED_UNTESTED_MIGRATION_NOT_APPLIED"
pg["LessonEntries"][0]["ReadyForStageD"]=False
progresspath.write_text(json.dumps(pg,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
print("G12_TEACHER_SUBMISSION_ROUTE_AUTHORED")
print("G12_RUBRIC_MIGRATION_WRITTEN_NOT_APPLIED")
