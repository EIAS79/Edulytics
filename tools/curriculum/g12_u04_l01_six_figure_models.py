from pathlib import Path
import json, datetime
root=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
base=root/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
ap=base/"u04-l01-source-inventory-and-corrected-boundary.json"
a=json.loads(ap.read_text(encoding="utf8"))
assert a["OriginalExerciseMathVerified"]==68 and a["IndependentExerciseTasksAuthored"]==68 and not a["ReadyForStageD"]
models=[
(1,"Basic exponential growth y=b^x; positive range, y-intercept and y=0 asymptote","Independent y=3^x with x=-2,0,1,2 giving y=1/9,1,3,9.","The graph rises with y-intercept (0,1) and approaches zero to the left; compare constant ratios.",1),
(2,"Compare exponential growth with linear growth","Independent A=20+5x and B=20(1.3)^x on x≥0.","Linear additive differences are constant; exponential ratios are constant and eventually grow faster.",1),
(3,"Fit exponential population model from initial and later samples","Independent P(t)=80(1.12)^t for t≥0, with initial P(0)=80.","Exponential model can be used for bounded time but indefinite biological forecasts need constraints.",61),
(4,"Exponential decay through two mathematical points","Independent y=2.5(0.65)^x, with values at x=0 and x=2.","Base between zero and one yields decay and horizontal asymptote y=0.",21),
(5,"Derive a graph's exponential equation from intercept and second point","Independent f(x)=2·2^x with points (0,2),(2,8).","Recover initial multiplier a from x=0; b from output ratio over two input steps.",22),
(6,"Analyze a noninteger initial value on an exponential graph","Independent f(x)=sqrt(2)·1.4^x, with f(0)=sqrt(2), f(1)=1.4sqrt(2).","The initial value can be irrational and model growth remains exponential.",18)
]
asset=base/"u04-l01-six-firstparty-figure-skill-alternatives.json"
asset.write_text(json.dumps({"SchemaVersion":1,"Source":"OpenStax 2017 Section 4.1 local PDF","SourceSha256":a["SourceSHA256"],"OriginalFigureCount":6,
"FirstPartyIndependentFigureSkills":[{"SourceFigureNumber":n,"Skill":skill,"IndependentModel":model,"Reasoning":reason,"RelatedOriginalExerciseSkill":exercise,"SourceGraphicCopied":False}for n,skill,model,reason,exercise in models],
"ExactPublisherGraphicParity":False,"StageD":"NOT_RUN"},ensure_ascii=False,indent=2)+"\n",encoding="utf8")
csharp="""using Edulytics.Core.Mathematics.Visuals;

namespace Edulytics.Web.Presentation;

/// <summary>First-party Section 4.1 source-skill graph alternatives.
/// They do not reproduce textbook illustrations. Reuses existing typed SVG
/// renderer; Stage D runtime has not been executed.</summary>
public static class Grade12PrecalculusSection41GraphModels
{
    public sealed record GraphTask(int OriginalSourceFigureSkill, string Prompt,
        CoordinatePlaneVisualSpec Graph);
    public static readonly IReadOnlyList<GraphTask> All = Build();

    private static IReadOnlyList<GraphTask> Build()
    {
        var items = new List<GraphTask>();
        Add(1, -2m, 3m, 0m, 30m, [x => (decimal)Math.Pow(3d,(double)x)],
            "Exponential growth: y=3^x, positive and never touching y=0.");
        Add(2, 0m, 5m, 0m, 100m,
            [x => 20m+5m*x, x => 20m*(decimal)Math.Pow(1.3d,(double)x)],
            "Compare linear 20+5x and exponential 20(1.3)^x.");
        Add(3, 0m, 8m, 0m, 230m,
            [x => 80m*(decimal)Math.Pow(1.12d,(double)x)],
            "Population P(t)=80(1.12)^t, t>=0; interpretation is a model, not indefinite truth.");
        Add(4, -3m, 4m, 0m, 20m,
            [x => 2.5m*(decimal)Math.Pow(.65d,(double)x)],
            "Decay y=2.5(0.65)^x; decreasing and bounded below by zero.");
        Add(5, -3m, 4m, 0m, 40m,
            [x => 2m*(decimal)Math.Pow(2d,(double)x)],
            "Use (0,2) and (2,8) to recover a=2 and b=2.");
        Add(6, -3m, 4m, 0m, 20m,
            [x => (decimal)Math.Sqrt(2d)*(decimal)Math.Pow(1.4d,(double)x)],
            "Graph with irrational initial value sqrt(2), growth factor 1.4.");
        return items;

        void Add(int n, decimal xmin, decimal xmax, decimal ymin,
            decimal ymax, Func<decimal, decimal>[] functions, string prompt)
        {
            var lines = new List<MathematicalSegment>();
            foreach (var f in functions)
            {
                MathematicalPoint? previous = null;
                for (var index=0; index<=120; index++)
                {
                    var x=xmin+(xmax-xmin)*index/120m;
                    var y=f(x);
                    if (y<ymin || y>ymax)
                    {
                        previous=null;
                        continue;
                    }
                    var point=new MathematicalPoint(x,y);
                    if (previous is not null)
                        lines.Add(new MathematicalSegment(previous,point));
                    previous=point;
                }
            }
            var model=new CoordinatePlaneVisualSpec(
                new MathematicalAxisWindow(xmin,xmax,ymin,ymax,1m,10m),
                [],lines,new MathematicalVisualAccessibility(
                    $"First-party exponential figure skill {n}",
                    prompt+" Independently authored mathematical graph, not an OpenStax image."));
            MathematicalVisualSpecValidator.Validate(model);
            items.Add(new GraphTask(n,prompt,model));
        }
    }
}
"""
cp=root/"src/Edulytics.Web/Presentation/Grade12PrecalculusSection41GraphModels.cs"
cp.write_text(csharp,encoding="utf8")
vp=root/"src/Edulytics.Web/Views/StudentPortal/Lesson.cshtml"
v=vp.read_text(encoding="utf8")
needle='        @if (Model.LessonCode == "PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:U03:L07")'
assert needle in v and "Grade12PrecalculusSection41GraphModels.All" not in v
insert="""        @* Independently authored Grade 12 exponential graphs for source skills.
           Mathematical drawings are teacher-reviewed; runtime is reserved for Stage D. *@
        @if (Model.LessonCode == "PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:U04:L01")
        {
            <section class="lesson-source-aligned-activities"
                     aria-label="Independent exponential mathematical graphs">
                <h2>Exponential functions: six independent teaching graphs</h2>
                <p>Investigate initial values, growth, decay, linear comparisons
                   and the horizontal asymptote. These independently authored
                   diagrams do not reproduce original textbook artwork.</p>
                @foreach (var task in Grade12PrecalculusSection41GraphModels.All)
                {
                    <figure class="student-card">
                        @Html.Raw(AdvancedMathematicsVisualRenderer.RenderSvg(task.Graph))
                        <figcaption>Figure skill @task.OriginalSourceFigureSkill:
                            @task.Prompt</figcaption>
                    </figure>
                }
            </section>
        }

"""
vp.write_text(v.replace(needle,insert+needle,1),encoding="utf8")
a["OriginalFigureSkillAlternatives"]=6
a["FigureSkillEvidencePath"]=asset.relative_to(root).as_posix()
a["IndependentGraphCodePath"]=cp.relative_to(root).as_posix()
a["GraphModelStudentViewCodeAuthored"]=True
a["GraphRuntimeVerified"]=False
a["BlockingReasons"]=["STAGE_C_TEACHER_ASSESSMENT_REUSE_ROUTING_PENDING","FINAL_LOCAL_ACADEMIC_CLOSURE_AND_GITHUB_PUSH_PENDING"]
a["UpdatedUtc"]=datetime.datetime.now(datetime.timezone.utc).isoformat()
ap.write_text(json.dumps(a,ensure_ascii=False,indent=2)+"\n",encoding="utf8")
print("U04L01_6_INDEPENDENT_TYPED_SVG_GRAPH_ALTERNATIVES_AUTHORED")
