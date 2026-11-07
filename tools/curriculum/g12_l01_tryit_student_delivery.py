from __future__ import annotations
from pathlib import Path
import json
root=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
base=root/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
original=base/"u01-l01-tryit-first-party-teacher-review.json"
data=json.loads(original.read_text(encoding="utf-8"))
assert len(data["Items"])==12
assert [r["SourceId"] for r in data["Items"]]==[f"G12-U01-L01-TRY{i:03d}" for i in range(1,13)]
def replace(path,a,b):
    p=root/path
    s=p.read_text(encoding="utf-8")
    assert s.count(a)==1, (path,a[:120],s.count(a))
    p.write_text(s.replace(a,b),encoding="utf-8",newline="\n")
path="src/Edulytics.Web/Presentation/Grade12PrecalculusSection11Exercises.cs"
snippet="    public static Task? Find(int number) =>\n"
parts=[]
for i,r in enumerate(data["Items"],start=1):
    num=93+i
    prompt=json.dumps(r["EdulyticsIndependentPrompt"],ensure_ascii=False)
    kind="DIAGRAM_AND_JUSTIFICATION" if "graph" in r["EdulyticsIndependentPrompt"].lower() or "sketch" in r["EdulyticsIndependentPrompt"].lower() else "WRITTEN_EXPLANATION"
    parts.append(f"        new({num}, \"Try It — independent practice\", {prompt}, \"TEACHER_REVIEW_REQUIRED\", \"{kind}\"),")
new="""    // Original Section 1.1 also contains twelve Try It learning actions.
    // Numbers 94–105 are Edulytics internal submission identifiers, not
    // publisher exercise numbers or additions to the 92 exercise denominator.
    public static readonly IReadOnlyList<Task> TryIts =
    [
"""+ "\n".join(parts) + """
    ];

    public static string DisplayLabel(int number) =>
        number == CoolDownNumber ? "Edulytics original Cool-down" :
        number is >= 94 and <= 105 ? $"Try It {number - 93}" :
        number is >= 1 and <= 92 ? $"Exercise {number}" : "Unknown activity";

    public static Task? Find(int number) =>
        number == CoolDownNumber ? CoolDown :
        number is >= 94 and <= 105
            ? TryIts.FirstOrDefault(task => task.Number == number)
            : All.FirstOrDefault(task => task.Number == number);
}
"""
p=root/path;s=p.read_text(encoding="utf-8")
assert s.count(snippet)==1
s=s[:s.index(snippet)]+new
p.write_text(s,encoding="utf-8",newline="\n")
path="src/Edulytics.Web/Views/StudentPortal/Lesson.cshtml"
anchor='''                <section class="student-card" aria-label="Edulytics original Cool-down">'''
html='''                <section class="student-card" aria-label="Edulytics first-party Try It activities">
                    <h3>Try It — independent practice</h3>
                    <p>12 new first-party tasks aligned to the section skills.
                       Written reasoning and graphs are reviewed by the assigned
                       mathematics teacher, not automatically scored.</p>
                    <ol>
                        @foreach (var task in Grade12PrecalculusSection11Exercises.TryIts)
                        {
                            <li>
                                <strong>Try It @(task.Number - 93):</strong> @task.Prompt
                                <a asp-controller="Grade12PrecalculusRubricStudent"
                                   asp-action="Index" asp-route-lessonId="@Model.Id"
                                   asp-route-exerciseNumber="@task.Number">
                                    Submit Try It @(task.Number - 93) for teacher review
                                </a>
                            </li>
                        }
                    </ol>
                </section>
'''
replace(path,anchor,html+anchor)
student="src/Edulytics.Web/Views/Grade12PrecalculusRubricStudent/Index.cshtml"
replace(student,'''@(Model.ExerciseNumber == Grade12PrecalculusSection11Exercises.CoolDownNumber
    ? "Edulytics original Cool-down" : $"Exercise {Model.ExerciseNumber}")''',
    '''@(Grade12PrecalculusSection11Exercises.DisplayLabel(Model.ExerciseNumber))''')
teach="src/Edulytics.Web/Views/Grade12PrecalculusRubricTeacher/Index.cshtml"
replace(teach,'''@(row.ExerciseNumber == 93 ? "Edulytics Cool-down" : $"Exercise {row.ExerciseNumber}")''',
        '''@(Grade12PrecalculusSection11Exercises.DisplayLabel(row.ExerciseNumber))''')
replace(teach,'''@model Edulytics.Web.Controllers.G12PrecalcRubricRow[]''',
    '''@model Edulytics.Web.Controllers.G12PrecalcRubricRow[]
@using Edulytics.Web.Presentation''')
review="src/Edulytics.Web/Views/Grade12PrecalculusRubricTeacher/Review.cshtml"
replace(review,'''@(Model.Submission.ExerciseNumber == 93
    ? "Edulytics original Cool-down" : $"exercise {Model.Submission.ExerciseNumber}")''',
    '''@(Grade12PrecalculusSection11Exercises.DisplayLabel(Model.Submission.ExerciseNumber))''')
replace(review,'''@model Edulytics.Web.Controllers.G12PrecalcRubricTeacherPage''',
    '''@model Edulytics.Web.Controllers.G12PrecalcRubricTeacherPage
@using Edulytics.Web.Presentation''')
migration="src/Edulytics.Data/Migrations/20261106193000_Grade12PrecalculusTeacherRubric.cs"
replace(migration,'CHECK ("ExerciseNumber" BETWEEN 1 AND 93)',
        'CHECK ("ExerciseNumber" BETWEEN 1 AND 105)')
tests="tests/Edulytics.Tests/MathematicsIntelligence/Grade12PrecalculusSection11SourceContractsTests.cs"
replace(tests,'''        Assert.Null(Grade12PrecalculusSection11Exercises.Find(94));''',
    '''        Assert.Equal(12, Grade12PrecalculusSection11Exercises.TryIts.Count);
        Assert.Equal(Enumerable.Range(94, 12),
            Grade12PrecalculusSection11Exercises.TryIts.Select(x => x.Number));
        Assert.NotNull(Grade12PrecalculusSection11Exercises.Find(94));
        Assert.Equal("Try It 1", Grade12PrecalculusSection11Exercises.DisplayLabel(94));
        Assert.Equal("Edulytics original Cool-down",
            Grade12PrecalculusSection11Exercises.DisplayLabel(93));
        Assert.Null(Grade12PrecalculusSection11Exercises.Find(106));''')
data["TeacherAssessmentRoute"]="EXERCISE_SCOPED_TEACHER_REVIEW_94_TO_105_AUTHORED_NOT_RUNTIME_TESTED"
data["SubmissionItemNumbers"]=list(range(94,106))
data["StudentViewWiredToFirstPartyTryItTasks"]=True
data["TeacherRubricPersisted"]=False
for i,r in enumerate(data["Items"]):
    r["StudentTaskSubmissionNumber"]=94+i
    r["StudentViewWired"]=True
    r["TeacherReviewRouteCodeAuthored"]=True
    r["TeacherRubricPersisted"]=False
original.write_text(json.dumps(data,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
asspath=base/"u01-l01-assessment.json"
ass=json.loads(asspath.read_text(encoding="utf-8"))
ass["FirstPartyTryIts"]={
    "Count":12,
    "OriginalSourceTryItCount":12,
    "TeacherSubmissionItemNumbers":[94,105],
    "StudentView":"src/Edulytics.Web/Views/StudentPortal/Lesson.cshtml",
    "Code":"src/Edulytics.Web/Presentation/Grade12PrecalculusSection11Exercises.cs",
    "Reviewer":"Assigned mathematics teacher",
    "Status":"AUTHORING_DELIVERY_AND_TEACHER_ROUTE_WRITTEN_NOT_RUNTIME_VERIFIED",
    "Tests":"WRITTEN_NOT_RUN",
    "SourceTryItSemanticParity":"NOT_CERTIFIED"
}
asspath.write_text(json.dumps(ass,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
cp_path=base/"u01-l01-abc-checkpoint.json"
cp=json.loads(cp_path.read_text(encoding="utf-8"))
cp["FirstPartyTryItStudentTasksWired"]=12
cp["FirstPartyTryItTeacherReviewIds"]="94-105"
cp["TeacherRubricSubmissionPersistence"]="G12_EXERCISES_1_TO_92_COOLDOWN_93_TRYITS_94_TO_105_AUTHORED_UNTESTED"
cp["ReadyForStageD"]=False
cp_path.write_text(json.dumps(cp,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
pr_path=base/"full-grade-progress.json"
progress=json.loads(pr_path.read_text(encoding="utf-8"))
p=progress["LessonEntries"][0]
p["FirstPartyTryItStudentTasksWired"]=12
p["TeacherRubricSubmissionPersistence"]="G12_EXERCISE_1_TO_92_COOLDOWN_93_TRYIT_94_TO_105_AUTHORED_UNTESTED"
p["ReadyForStageD"]=False
pr_path.write_text(json.dumps(progress,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
print("G12_TRYIT_12_STUDENT_AND_TEACHER_ROUTE_AUTHORED; SOURCE_EXERCISES_STILL_92")
