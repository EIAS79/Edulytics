from pathlib import Path
import json,datetime
root=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
base=root/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
ap=base/"u03-l07-source-inventory-and-gap-audit.json"
tp=root/"src/Edulytics.Core/Curriculum/LessonContent/Assessment/us-ccss-g12-precalculus-u03-l07-firstparty-teacher-led.json"
a=json.loads(ap.read_text(encoding="utf-8"));t=json.loads(tp.read_text(encoding="utf-8"))
assert len(t["FirstPartyIndependentStudentMathQuestions"])==80
graphs=[
(57,"(x−1)/((x+2)(x−3))","x=−2,3","x=1","y=0","(0,1/6)",[-2,3],"x => (x-1m)/((x+2m)*(x-3m))","Simple zero; two vertical poles; numerator degree lower."),
(58,"(x+2)/((x+3)(x−2))","x=−3,2","x=−2","y=0","(0,−1/3)",[-3,2],"x => (x+2m)/((x+3m)*(x-2m))","Simple zero between two vertical poles."),
(59,"(x−2)/((x+1)(x−4))","x=−1,4","x=2","y=0","(0,1/2)",[-1,4],"x => (x-2m)/((x+1m)*(x-4m))","Two vertical poles and a middle-branch x-axis crossing."),
(60,"(x+1)/((x+3)(x−4))","x=−3,4","x=−1","y=0","(0,−1/12)",[-3,4],"x => (x+1m)/((x+3m)*(x-4m))","Three separated branches with a single zero."),
(61,"(x−1)/(x−2)","x=2","x=1","y=1","(0,1/2)",[2],"x => (x-1m)/(x-2m)","One vertical pole and nonzero horizontal end level."),
(62,"(x+3)/(x+1)","x=−1","x=−3","y=1","(0,3)",[-1],"x => (x+3m)/(x+1m)","One vertical pole, horizontal end level one."),
(63,"(x−2)²/((x+2)(x−4))","x=−2,4","x=2 double","y=1","(0,−1/2)",[-2,4],"x => ((x-2m)*(x-2m))/((x+2m)*(x-4m))","Middle branch has an even-multiplicity contact at x=2."),
(64,"(x−1)²/((x+3)(x−4))","x=−3,4","x=1 double","y=1","(0,−1/12)",[-3,4],"x => ((x-1m)*(x-1m))/((x+3m)*(x-4m))","Middle branch touches the x-axis at x=1.")
]
# Pure numerical checks of some reference y-intercepts.
from fractions import Fraction
assert Fraction(-1,(2)*(-3))==Fraction(1,6)
assert Fraction(2,(3)*(-2))==Fraction(-1,3)
assert Fraction(-2,(1)*(-4))==Fraction(1,2)
assert Fraction(1,(3)*(-4))==Fraction(-1,12) # source graph #60 value is negative, correct below

for n,formula,vas,zeros,ha,yint,poles,func,reading in graphs:
 assert n in range(57,65)
 q=f"Interpret the independently authored graph labelled Exercise {n}. State one rational formula consistent with the plotted branches, all x-intercepts (with multiplicities), vertical asymptotes and horizontal asymptote."
 expected=f"One consistent first-party function is f(x)={formula}; vertical asymptotes {vas}; x-intercept(s) {zeros}; horizontal asymptote {ha}; y-intercept {yint}. Explain each factor using crossings/tangencies and branch behavior."
 t["FirstPartyIndependentStudentMathQuestions"].append({"SourceSkillReferenceExercise":n,"IndependentStudentQuestion":q,"TeacherExpectedMath":expected,"MarkingMode":"TEACHER_REVIEW_GRAPH_INTERPRETATION","AcademicQuestionStatus":"INDEPENDENT_AUTHORING_AND_TYPED_GRAPH_MODEL","LinkedGraphSource":"Grade12PrecalculusSection37GraphModels","Runtime":"NOT_RUN"})
 item=a["SourceNumberedExerciseLedger"][n-1]
 assert item["Number"]==n
 item["FirstPartyIndependentMathQuestionVerified"]=True
 item["FirstPartyIndependentQuestionPath"]=tp.relative_to(root).as_posix()
 item["IndependentGraphModel"]=formula
assert len(t["FirstPartyIndependentStudentMathQuestions"])==88
cs_head="""using Edulytics.Core.Mathematics.Visuals;

namespace Edulytics.Web.Presentation;

/// <summary>
/// First-party independently authored Section 3.7 rational-function graphs.
/// Each model supports a source-equivalent graph-reading exercise, not copied
/// textbook pixels or claims of numerical identity with source graphs 57–64.
/// Reuses existing typed SVG graph renderer. Stage D not run.
/// </summary>
public static class Grade12PrecalculusSection37GraphModels
{
    public sealed record GraphTask(int ExerciseNumber, string Prompt,
        CoordinatePlaneVisualSpec Graph);
    public static readonly IReadOnlyList<GraphTask> All = Build();
    private static IReadOnlyList<GraphTask> Build()
    {
        var tasks = new List<GraphTask>();
"""
cs_lines=[]
for n,formula,vas,zeros,ha,yint,poles,func,reading in graphs:
 pole_str=", ".join(str(v)+"m" for v in poles)
 cs_lines.append(f'        Add({n}, {func}, [{pole_str}],')
 cs_lines.append(f'            "Infer one factored formula, roots and asymptotes from this independent graph.",')
 cs_lines.append(f'            "{reading}");')
cs_tail="""
        return tasks;

        void Add(int exercise, Func<decimal, decimal> evaluate,
            decimal[] excludedPoles, string prompt, string accessibleFeature)
        {
            var segments = new List<MathematicalSegment>();
            MathematicalPoint? previous = null;
            decimal? previousX = null;
            for (var index = 0; index <= 140; index++)
            {
                var x = -7m + index / 10m;
                // Do not bridge real poles with a straight segment.
                if (excludedPoles.Any(p => Math.Abs(x - p) < 0.17m))
                {
                    previous = null;
                    previousX = null;
                    continue;
                }
                var y = evaluate(x);
                if (y < -6m || y > 6m)
                {
                    previous = null;
                    previousX = null;
                    continue;
                }
                var point = new MathematicalPoint(x, y);
                if (previous is not null && previousX is decimal lastX && x - lastX <= 0.11m)
                    segments.Add(new MathematicalSegment(previous, point));
                previous = point;
                previousX = x;
            }
            var drawing = new CoordinatePlaneVisualSpec(
                new MathematicalAxisWindow(-7m, 7m, -6m, 6m, 1m, 1m),
                [], segments,
                new MathematicalVisualAccessibility(
                    $"Independent rational graph exercise {exercise}",
                    accessibleFeature + " The curves are separated at excluded poles, " +
                    "and no textbook figure has been copied."));
            MathematicalVisualSpecValidator.Validate(drawing);
            tasks.Add(new GraphTask(exercise, prompt, drawing));
        }
    }
}
"""
cs=cs_head+"\n".join(cs_lines)+"\n"+cs_tail
cs_path=root/"src/Edulytics.Web/Presentation/Grade12PrecalculusSection37GraphModels.cs"
cs_path.write_text(cs,encoding="utf-8")
view_path=root/"src/Edulytics.Web/Views/StudentPortal/Lesson.cshtml"
view=view_path.read_text(encoding="utf-8")
needle='        @if (Model.LessonCode == "PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:U01:L01")'
assert needle in view and "Grade12PrecalculusSection37GraphModels.All" not in view
addition="""        @* Grade 12 Section 3.7: reuse the typed mathematics renderer for eight
           independently authored graph interpretation tasks. No Stage D claim. *@
        @if (Model.LessonCode == "PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:U03:L07")
        {
            <section class="lesson-source-aligned-activities"
                     aria-label="Independent rational graph interpretation practice">
                <h2>Rational functions: interpret original Edulytics graphs</h2>
                <p>Study each of these first-party teaching graphs. State a possible
                   factored rational expression, its roots, and its vertical and
                   horizontal asymptotes. Explain your reasoning to your teacher.
                   These diagrams are not copied from OpenStax.</p>
                @foreach (var task in Grade12PrecalculusSection37GraphModels.All)
                {
                    <figure class="student-card">
                        @Html.Raw(AdvancedMathematicsVisualRenderer.RenderSvg(task.Graph))
                        <figcaption>Independent practice @task.ExerciseNumber:
                            @task.Prompt</figcaption>
                    </figure>
                }
            </section>
        }

"""
view_path.write_text(view.replace(needle,addition+needle,1),encoding="utf-8")
t["FirstPartyIndependentStudentQuestionsAuthored"]=88
t["OriginalExerciseFirstPartyCoverageStatus"]="88_OF_88_INDEPENDENT_EXERCISE_ALTERNATIVES_AUTHORED_EIGHT_TYPED_GRAPH_MODELS_RUNTIME_NOT_VERIFIED"
t["OriginalPublisherGraphicBitmapsCopied"]=False
tp.write_text(json.dumps(t,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
a["OriginalExerciseAlternativesReviewed"]=88
a["FirstPartyIndependentMathQuestionCount"]=88
a["IndependentGraphTaskCount"]=8
a["IndependentGraphModelCodePath"]=cs_path.relative_to(root).as_posix()
a["GraphTaskStudentViewWiring"]="AUTHORED_NOT_RUNTIME_VERIFIED"
a["BlockingReasons"]=["23_SOURCE_FIGURE_SKILL_CROSSWALK_AND_INDEPENDENT_ALTERNATIVES_PENDING","STAGE_C_REUSE_EXTEND_ASSESSMENT_CONTRACT_PENDING","GRAPH_STUDENT_RENDERING_NOT_RUNTIME_TESTED_RESERVED_FOR_STAGE_D"]
a["ReadyForStageD"]=False
a["UpdatedAtUtc"]=datetime.datetime.now(datetime.timezone.utc).isoformat()
ap.write_text(json.dumps(a,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
print("U03_L07_ALL_88_INDEPENDENT_TASKS_EIGHT_FIRST_PARTY_GRAPH_MODELS_AUTHORED_NO_RUNTIME_TESTS")
