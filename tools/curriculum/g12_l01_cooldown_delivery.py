from pathlib import Path
import json
root=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
def fix(path,a,b):
    f=root/path
    s=f.read_text(encoding="utf-8")
    n=s.count(a)
    if n!=1: raise ValueError(f"{path}: anchor count {n}: {a[:120]!r}")
    f.write_text(s.replace(a,b),encoding="utf-8",newline="\n")

bank="src/Edulytics.Web/Presentation/Grade12PrecalculusSection11Exercises.cs"
fix(bank,'''    ];
}''','''    ];

    // Original Section 1.1 has exactly 92 numbered exercises.
    // Item 93 is an explicitly separate Edulytics first-party Cool-down.
    public const int CoolDownNumber = 93;

    public static readonly Task CoolDown = new(
        CoolDownNumber,
        "Edulytics original Cool-down",
        "(a) Decide whether R={(1,2),(1,4),(2,4)} is a function and explain. " +
        "(b) For f(x)=x²−2x+1, compute f(−1) with substitutions. " +
        "(c) Is h(x)=x² one-to-one when its domain is x≥0? Prove or explain.",
        "TEACHER_REVIEW_REQUIRED", "MULTIPART_WRITTEN_PROOF");

    public static Task? Find(int number) =>
        number == CoolDownNumber ? CoolDown :
        All.FirstOrDefault(task => task.Number == number);
}''')
controller="src/Edulytics.Web/Controllers/Grade12PrecalculusRubricController.cs"
fix(controller,'''        var task = Grade12PrecalculusSection11Exercises.All
            .FirstOrDefault(item => item.Number == exerciseNumber);''','''        var task = Grade12PrecalculusSection11Exercises.Find(exerciseNumber);''')
fix(controller,'''        if (!Grade12PrecalculusSection11Exercises.All.Any(x => x.Number == exerciseNumber))
            return NotFound();''','''        if (Grade12PrecalculusSection11Exercises.Find(exerciseNumber) is null)
            return NotFound();''')

studentview="src/Edulytics.Web/Views/Grade12PrecalculusRubricStudent/Index.cshtml"
fix(studentview,'''<h1>Precalculus: teacher-reviewed mathematical response</h1>
<p><strong>Exercise @Model.ExerciseNumber</strong>: @Model.Prompt</p>''','''<h1>Precalculus: teacher-reviewed mathematical response</h1>
<p><strong>@(Model.ExerciseNumber == Grade12PrecalculusSection11Exercises.CoolDownNumber
    ? "Edulytics original Cool-down" : $"Exercise {Model.ExerciseNumber}")</strong>: @Model.Prompt</p>''')
fix(studentview,'''@model Edulytics.Web.Controllers.G12PrecalcRubricStudentPage''','''@model Edulytics.Web.Controllers.G12PrecalcRubricStudentPage
@using Edulytics.Web.Presentation''')
teacherview="src/Edulytics.Web/Views/Grade12PrecalculusRubricTeacher/Index.cshtml"
fix(teacherview,'''            Exercise @row.ExerciseNumber · Lesson @row.LessonId''','''            @(row.ExerciseNumber == 93 ? "Edulytics Cool-down" : $"Exercise {row.ExerciseNumber}") · Lesson @row.LessonId''')
teacherreview="src/Edulytics.Web/Views/Grade12PrecalculusRubricTeacher/Review.cshtml"
fix(teacherreview,'''<h1>Section 1.1, exercise @Model.Submission.ExerciseNumber — teacher assessment</h1>''','''<h1>Section 1.1, @(Model.Submission.ExerciseNumber == 93
    ? "Edulytics original Cool-down" : $"exercise {Model.Submission.ExerciseNumber}") — teacher assessment</h1>''')

# SQL database constraint: 92 publisher exercises + one Edulytics-only cool-down.
mig="src/Edulytics.Data/Migrations/20261106193000_Grade12PrecalculusTeacherRubric.cs"
# File is sparse; materialize via tracked git show, not modify the database.
import subprocess
mfile=root/mig
mfile.parent.mkdir(parents=True,exist_ok=True)
text=subprocess.check_output(["git","-C",str(root),"show","HEAD:"+mig],encoding="utf-8")
assert text.count('CHECK ("ExerciseNumber" BETWEEN 1 AND 92)')==1
mfile.write_text(text.replace('CHECK ("ExerciseNumber" BETWEEN 1 AND 92)',
    'CHECK ("ExerciseNumber" BETWEEN 1 AND 93)'),encoding="utf-8",newline="\n")

view="src/Edulytics.Web/Views/StudentPortal/Lesson.cshtml"
fix(view,'''                }
            </section>
        }
        @if (Model.RichContent is not null)
        {''','''                }
                <section class="student-card" aria-label="Edulytics original Cool-down">
                    <h3>Cool-down — explain what you know</h3>
                    <p>@Grade12PrecalculusSection11Exercises.CoolDown.Prompt</p>
                    <a asp-controller="Grade12PrecalculusRubricStudent" asp-action="Index"
                       asp-route-lessonId="@Model.Id"
                       asp-route-exerciseNumber="@Grade12PrecalculusSection11Exercises.CoolDownNumber">
                        Submit the Cool-down for teacher review
                    </a>
                </section>
            </section>
        }
        @if (Model.RichContent is not null)
        {''')
test="tests/Edulytics.Tests/MathematicsIntelligence/Grade12PrecalculusSection11SourceContractsTests.cs"
fix(test,'''        Assert.Equal(92, tasks.Count);
        Assert.Equal(Enumerable.Range(1, 92), tasks.Select(x => x.Number));''','''        Assert.Equal(92, tasks.Count);
        Assert.Equal(Enumerable.Range(1, 92), tasks.Select(x => x.Number));
        Assert.Equal(93, Grade12PrecalculusSection11Exercises.CoolDown.Number);
        Assert.Contains("one-to-one", Grade12PrecalculusSection11Exercises.CoolDown.Prompt);
        Assert.Null(Grade12PrecalculusSection11Exercises.Find(94));''')

base=root/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
assessment=base/"u01-l01-assessment.json"
j=json.loads(assessment.read_text(encoding="utf-8"))
j["FirstPartyCoolDown"]["DeliveryStatus"]="STUDENT_VIEW_AND_TEACHER_PER_EXERCISE_SUBMISSION_CODE_AUTHORED_NOT_RUNTIME_VERIFIED"
j["FirstPartyCoolDown"]["TeacherReviewItemNumber"]=93
j["FirstPartyCoolDown"]["SeparateFromOriginalSectionExerciseCount"]=True
j["FirstPartyCoolDown"]["DatabaseMigration"]="WRITTEN_NOT_APPLIED"
assessment.write_text(json.dumps(j,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
checkpoint=base/"u01-l01-abc-checkpoint.json"
c=json.loads(checkpoint.read_text(encoding="utf-8"))
c["FirstPartyCoolDownStudentViewWiring"]="AUTHORED_NOT_RUNTIME_VERIFIED"
c["FirstPartyCoolDownTeacherReviewNumber"]=93
c["ReadyForStageD"]=False
checkpoint.write_text(json.dumps(c,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
progress=base/"full-grade-progress.json"
p=json.loads(progress.read_text(encoding="utf-8"))
p["LessonEntries"][0]["FirstPartyCoolDownStudentViewWiring"]="AUTHORED_NOT_RUNTIME_VERIFIED"
p["LessonEntries"][0]["ReadyForStageD"]=False
progress.write_text(json.dumps(p,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
print("G12_SOURCE_EXERCISES=92; FIRST_PARTY_COOLDOWN=93; ALL_TEACHER_SCOPED")
