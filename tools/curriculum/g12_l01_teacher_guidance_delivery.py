from pathlib import Path
import json
root=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
base=root/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
routing=json.loads((base/"u01-l01-exercise-routing.json").read_text(encoding="utf-8"))
tryits=json.loads((base/"u01-l01-tryit-first-party-teacher-review.json").read_text(encoding="utf-8"))
assessment=json.loads((base/"u01-l01-assessment.json").read_text(encoding="utf-8"))
assert len(routing["SourceExerciseContracts"])==92
assert len(tryits["Items"])==12
items=[]
for r in routing["SourceExerciseContracts"]:
    n=r["OriginalExerciseNumber"]
    explanation=(r.get("OriginalMathematicsSourceReview") or "").strip()
    if n==52:
        explanation=("Graph reading: f(−1) is approximately 1. The exact input for "
            "f(x)=3 cannot be certified from the finite plotted window alone; "
            "do not invent a numerical root without a warranted extension.")
    elif n==53:
        explanation="From the plotted W-curve, f(0)=1; f(x)=−3 occurs at x=−2 and x=2."
    elif n==54:
        explanation="From the inverted-V, f(4)=−3 and f(x)=1 occurs at x=−6 and x=0."
    assert explanation,(n,r)
    graph=n in set(range(40,60)) | set(range(76,88))
    caution=("The original source diagram/window and the independent student teaching graphic "
        "are not guaranteed pixel- or mathematically identical; verify axes, domain and scale. "
        "The original graph for exercise 59 uses π-radian horizontal ticks." if graph else
        "Check all subparts, valid domains, physical units and requested justifications; "
        "this is teacher marking guidance, not an automatic scoring decision.")
    if n==52:
        caution="SOURCE_GRAPH_UNRESOLVED: no exact numerical root for f(x)=3 may be invented from the printed window. Teacher must judge graphical precision and explanation."
    items.append({"ActivityNumber":n,"Expected":explanation,"Caution":caution,
                  "OriginalSourceSectionExercise":True,
                  "SourceMathVerification":r["SourceMathVerificationStatus"]})
items.append({"ActivityNumber":93,"Expected":assessment["FirstPartyCoolDown"]["Expected"],
              "Caution":"Original Edulytics Cool-down; not a numbered publisher exercise. Four rubric criteria, teacher judgement only.",
              "OriginalSourceSectionExercise":False,"SourceMathVerification":"FIRST_PARTY"})
for i,r in enumerate(tryits["Items"],1):
    items.append({"ActivityNumber":93+i,"Expected":r["ExpectedTeacherAnswer"],
        "Caution":"Independent Edulytics Try It analogue; do not claim verbatim original source material or automatic scoring.",
        "OriginalSourceSectionExercise":False,"SourceMathVerification":"FIRST_PARTY"})
assert len(items)==105 and [x["ActivityNumber"] for x in items]==list(range(1,106))
assert "tons per week" in items[87]["Expected"]
assert "π" in items[58]["Caution"]
assert "UNRESOLVED" in items[51]["Caution"]
path=root/"src/Edulytics.Web/Presentation/Grade12PrecalculusSection11TeacherGuidance.cs"
lines=[
"namespace Edulytics.Web.Presentation;",
"",
"/// <summary>",
"/// Teacher-only reference for Section 1.1 authored mathematical explanations.",
"/// Never render in the learner view or use as a machine-scoring answer key.",
"/// The 92 source exercise numbers and 13 first-party activities are distinct.",
"/// </summary>",
"public static class Grade12PrecalculusSection11TeacherGuidance",
"{",
"    public sealed record Note(int Number, string ExpectedMathematics,",
"        string Caveat, bool OriginalSourceExercise);",
"",
"    private static readonly IReadOnlyDictionary<int, Note> Notes =",
"        new Dictionary<int, Note>",
"        {"
]
for x in items:
    n=x["ActivityNumber"]
    ans=json.dumps(x["Expected"],ensure_ascii=False)
    caution=json.dumps(x["Caution"],ensure_ascii=False)
    original="true" if x["OriginalSourceSectionExercise"] else "false"
    lines.append(f"            [{n}] = new({n}, {ans}, {caution}, {original}),")
lines+=["        };","","    public static int Count => Notes.Count;","","    public static Note? Find(int number) =>",
        "        Notes.TryGetValue(number, out var note) ? note : null;","}",""]
path.write_text("\n".join(lines),encoding="utf-8",newline="\n")
# Only the role- and class-authorized teacher review view gets the reference guidance.
viewpath=root/"src/Edulytics.Web/Views/Grade12PrecalculusRubricTeacher/Review.cshtml"
view=viewpath.read_text(encoding="utf-8")
anchor='''<pre style="white-space:pre-wrap">@Model.Submission.ResponseText</pre>'''
assert view.count(anchor)==1
view=view.replace(anchor,anchor+'''
@if (Grade12PrecalculusSection11TeacherGuidance.Find(Model.Submission.ExerciseNumber) is { } note)
{
    <section class="student-card" aria-label="Teacher-only mathematical review guidance">
        <h2>Teacher-only mathematical review guidance</h2>
        <p>@note.ExpectedMathematics</p>
        <p><em>@note.Caveat</em></p>
        <p>This reference is not an automatic grade. Inspect the student's own
           proof, diagram and all requested subparts before awarding marks.</p>
    </section>
}''')
viewpath.write_text(view,encoding="utf-8",newline="\n")
testpath=root/"tests/Edulytics.Tests/MathematicsIntelligence/Grade12PrecalculusSection11SourceContractsTests.cs"
test=testpath.read_text(encoding="utf-8")
anchor='''    [Fact]
    public void IndependentWorkedExamplesHaveCorrectMathGoldenValues()'''
assert test.count(anchor)==1
test=test.replace(anchor,'''    [Fact]
    public void Section11TeacherReviewGuidanceCoversAll105WithoutExposingOriginalFigureExactRoots()
    {
        Assert.Equal(105, Grade12PrecalculusSection11TeacherGuidance.Count);
        Assert.Contains("tons per week",
            Grade12PrecalculusSection11TeacherGuidance.Find(88)!.ExpectedMathematics,
            StringComparison.Ordinal);
        Assert.Contains("UNRESOLVED",
            Grade12PrecalculusSection11TeacherGuidance.Find(52)!.Caveat,
            StringComparison.Ordinal);
        Assert.False(Grade12PrecalculusSection11TeacherGuidance.Find(94)!.OriginalSourceExercise);
        Assert.Null(Grade12PrecalculusSection11TeacherGuidance.Find(106));
    }

'''+anchor)
testpath.write_text(test,encoding="utf-8",newline="\n")
a=assessment
a["TeacherReview"]["AuthoredTeacherOnlyAnswerGuidance"]={
    "Activities":105,"OriginalSectionExerciseKeys":92,
    "OriginalEdulyticsCoolDown":93,"FirstPartyTryItKeys":"94-105",
    "Code":"src/Edulytics.Web/Presentation/Grade12PrecalculusSection11TeacherGuidance.cs",
    "RoleProtectedView":"src/Edulytics.Web/Views/Grade12PrecalculusRubricTeacher/Review.cshtml",
    "TeacherScope":"MAY_REVIEW_SCHOOL_CLASS_LESSON_ENROLLMENT",
    "SourceGraph52B":"UNRESOLVED_EXACT_VALUE_NO_AUTOSCORING",
    "Delivery":"AUTHORED_NOT_RUNTIME_VERIFIED",
    "Tests":"WRITTEN_NOT_RUN"
}
(base/"u01-l01-assessment.json").write_text(json.dumps(a,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
checkpoint=base/"u01-l01-abc-checkpoint.json"
cp=json.loads(checkpoint.read_text(encoding="utf-8"))
cp["TeacherOnlyAnswerGuidanceAuthoredActivities"]=105
cp["TeacherReviewGrade12SourceCode"]="AUTHORED_UNTESTED"
cp["ReadyForStageD"]=False
checkpoint.write_text(json.dumps(cp,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
progress=base/"full-grade-progress.json"
pg=json.loads(progress.read_text(encoding="utf-8"))
pg["LessonEntries"][0]["TeacherOnlyAnswerGuidanceAuthoredActivities"]=105
pg["LessonEntries"][0]["ReadyForStageD"]=False
progress.write_text(json.dumps(pg,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
print("TEACHER_ONLY_MATH_GUIDANCE_105_ENTRIES_AUTHORED_52_B_UNRESOLVED")
